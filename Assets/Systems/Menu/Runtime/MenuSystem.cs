using DG.Tweening;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Systems.Runtime;
using System.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Reflex.Core;
using Reflex.Runtime;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Systems.Menu.Runtime
{
	public sealed class MenuSystem : MonoSystem, IInjectable
	{
		private const string MENU_LOAD_PATH = "Assets/Menus";

		public readonly MenuNames Names = new();

		[SerializeField] private Image _backgroundBlocker = default;
		[SerializeField] private Transform _menuRoot = default;
		[SerializeField] private List<Menu> _serializedMenuCache = default;

		private Container _injectionContainer = default;
		private readonly SemaphoreSlim _semaphore = new(1, 1);

		/// <summary>
		/// Inject Container
		/// </summary>
		public void OnInject(Container container) => _injectionContainer = container;

		/// <summary>
		/// Loads a menu with given assetName and provided Data
		/// </summary>
		public async UniTask LoadMenu<TData>(string assetName, TData data) where TData : IMenuData
		{
			await _semaphore.WaitAsync();

			try
			{
				var menu = await GetMenu(assetName);

				if (menu != default)
				{
					_serializedMenuCache.Add(menu);


					var menuObj = menu.gameObject;

					menuObj.SetActive(false);

					menu.OnInject(_injectionContainer);

					if (menu is IMenuDataReceiver<TData> dataReceiver)
					{
						dataReceiver.SetData(data);
					}

					menu.OnEnter();
					_serializedMenuCache.Add(menu);

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
			if (_serializedMenuCache.Count > 10)
			{
				while (_serializedMenuCache.Count != 0)
				{
					_serializedMenuCache.RemoveAt(_serializedMenuCache.Count - 1);
				}
			}
			
			var menu = _serializedMenuCache.FirstOrDefault(x => string.Equals(x.AssetName, assetName));

			if (menu == default)
			{
				var menuPath = $"{MENU_LOAD_PATH}/{assetName}.prefab";
				var opHandle = Addressables.LoadAssetAsync<GameObject>(menuPath);

				await opHandle;

				if (opHandle.Status == AsyncOperationStatus.Succeeded)
				{
					var menuObj = Instantiate(opHandle.Result, _menuRoot);
					menu = menuObj.GetComponent<Menu>();
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
				var count = _serializedMenuCache.Count;
				if (count > 0) _serializedMenuCache[count - 1].OnExit();
			}
			else
			{
				var menu = _serializedMenuCache.FirstOrDefault(x => x.AssetName == assetKey);
				if (menu == default) return;

				menu.OnExit();

				menu.transform.DOScale(0, 0.1f).From(1).SetEase(Ease.InBack)
					.OnComplete(() => { menu.gameObject.SetActive(false); });
			}
		}

		/// <summary>
		/// Unloads provided Menu
		/// </summary>
		/// <param name="menu"></param>
		public void UnLoadMenu(Menu menu = default)
		{
			if (menu == default)
			{
				var count = _serializedMenuCache.Count;
				if (count > 0) _serializedMenuCache[count - 1].OnExit();
			}
			else
			{
				menu.OnExit();

				menu.transform.DOScale(0, 0.1f).From(1).SetEase(Ease.InBack)
					.OnComplete(() => { menu.gameObject.SetActive(false); });
			}
		}
	}
}
