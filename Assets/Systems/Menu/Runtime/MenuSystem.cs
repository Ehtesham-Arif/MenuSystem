﻿using System;
using DG.Tweening;
using UnityEngine;
using System.Linq;
using Systems.Runtime;
using Reflex.Core;
using Reflex.Runtime;
using System.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Systems.Widgets.Runtime;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Systems.Menu.Runtime
{
	public sealed class MenuSystem : MonoSystem, IInjectable
	{
		private const string MENU_LOAD_PATH = "Assets/Menus";

		public readonly MenuNames Names = new();

		[SerializeField] private GameObject _transparentBlocker = default;
		[SerializeField] private UxButton _backgroundBlocker = default;
		[SerializeField] private Transform _menuRoot = default;
		[SerializeField] private List<Menu> _serializedMenuCache = default;

		private readonly SemaphoreSlim _semaphore = new(1, 1);
		private readonly List<Menu> _menuTrack = new();
		private Container _injectionContainer = default;
		private int _menuCount = 0;

		private void OnEnable() => _backgroundBlocker.SubscribeClick(OnBackgroundClick);

		private void OnDisable() => _backgroundBlocker.Dispose();

		private void OnBackgroundClick() => UnLoadMenu(default(Menu));

		/// <summary>
		/// Inject Container
		/// </summary>
		public void OnInject(Container container)
		{
			_injectionContainer = container;
		}

		/// <summary>
		/// Loads a menu with given assetName and provided Data
		/// </summary>
		public async UniTask LoadMenu<TData>(string assetName, TData data) where TData : IMenuData
		{
			await _semaphore.WaitAsync();

			try
			{
				// do not allow clicks during async push
				_transparentBlocker.SetActive(true);
				var menu = await GetMenu(assetName);
				_transparentBlocker.SetActive(false);

				if (menu != default)
				{
					var menuObj = menu.gameObject;

					menuObj.SetActive(false);

					menu.OnInject(_injectionContainer);

					if (menu is IMenuDataReceiver<TData> dataReceiver)
					{
						dataReceiver.SetData(data);
					}

					menu.OnEnter();
					_menuCount++;

					_menuTrack.Add(menu);
					SetBgState();

					if (_serializedMenuCache.Contains(menu) == false)
					{
						_serializedMenuCache.Add(menu);
					}

					menuObj.SetActive(true);

					menu.transform.DOScale(1, 0.1f).From(0).SetEase(Ease.InBack);
				}
			}
			finally
			{
				_semaphore.Release();
			}
		}

		private async UniTask<Menu> GetMenu(string assetName)
		{
			// Asset Key of provided menu prefab
			var assetKey = $"{MENU_LOAD_PATH}/{assetName}.prefab";

			// Count is exceeding 10 Clear Cache before instantiating new ones
			if (_serializedMenuCache.Count > 10)
			{
				while (_serializedMenuCache.Count != 0)
				{
					var cacheMenu = _serializedMenuCache[^1];
					_serializedMenuCache.Remove(cacheMenu);

					Destroy(cacheMenu.gameObject);
				}
			}

			var menu = _serializedMenuCache.FirstOrDefault(x => string.Equals(x.AssetKey, assetKey));

			if (menu == default)
			{
				var opHandle = Addressables.LoadAssetAsync<GameObject>(assetKey);

				await opHandle;

				if (opHandle.Status == AsyncOperationStatus.Succeeded)
				{
					var menuObj = Instantiate(opHandle.Result, _menuRoot);
					menu = menuObj.GetComponent<Menu>();
					menu.AssetKey = assetKey;
				}
			}

			return menu;
		}

		/// <summary>
		/// Unloads Menu with provided assetKey
		/// </summary>
		public void UnLoadMenu(string assetKey = null)
		{
			if (assetKey == default)
			{
				UnloadMenuFromTop();
			}
			else
			{
				var menu = _serializedMenuCache.FirstOrDefault(x => x.AssetKey == assetKey);
				ExitMenu(menu);
			}
		}

		/// <summary>
		/// Unloads provided Menu
		/// </summary>
		/// <param name="menu"></param>
		public void UnLoadMenu(Menu menu = default)
		{
			if (menu == default) UnloadMenuFromTop();
			else ExitMenu(menu);
		}

		// Unloads Last Loaded Menu
		private void UnloadMenuFromTop()
		{
			var count = _menuTrack.Count;

			if (count > 0)
			{
				var targetMenu = _menuTrack[count - 1];
				ExitMenu(targetMenu);
			}
		}

		private void ExitMenu(Menu menu)
		{
			if (menu == default) return;
			if (menu.AllowPop == false) return;

			menu.OnExit();
			_menuCount--;
			_menuTrack.Remove(menu);

			var disposables = menu.GetComponentsInChildren<IDisposable>();
			foreach (var disposable in disposables) disposable.Dispose();

			menu.transform.DOScale(0, 0.1f).From(1).SetEase(Ease.InBack)
				.OnComplete(() =>
				{
					SetBgState();
					menu.gameObject.SetActive(false);
				});
		}

		private void SetBgState() => _backgroundBlocker.gameObject.SetActive(_menuCount != 0);
	}
}
