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

		[SerializeField] private UxButton _backgroundBlocker = default;
		[SerializeField] private Transform _menuRoot = default;
		[SerializeField] private List<Menu> _serializedMenuCache = default;

		private Container _injectionContainer = default;
		private readonly SemaphoreSlim _semaphore = new(1, 1);
		private int _menuCount = 0;

		private void OnEnable()
		{
			_backgroundBlocker.SubscribeClick(OnBackgroundClick);
		}

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
				var menu = await GetMenu(assetName);

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

			var menu = _serializedMenuCache.FirstOrDefault(x => string.Equals(x.AssetName, assetName));

			if (menu == default)
			{
				SetBgState(true);

				var menuPath = $"{MENU_LOAD_PATH}/{assetName}.prefab";
				var opHandle = Addressables.LoadAssetAsync<GameObject>(menuPath);

				await opHandle;

				if (opHandle.Status == AsyncOperationStatus.Succeeded)
				{
					var menuObj = Instantiate(opHandle.Result, _menuRoot);
					menu = menuObj.GetComponent<Menu>();
				}
				else
				{
					SetBgState(false);
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
				SetBgState();
			}
			else
			{
				var menu = _serializedMenuCache.FirstOrDefault(x => x.AssetName == assetKey);
				if (menu == default) return;

				if (menu.AllowPop)
				{
					menu.OnExit();
					_menuCount--;
				}

				menu.transform.DOScale(0, 0.1f).From(1).SetEase(Ease.InBack)
					.OnComplete(() =>
					{
						menu.gameObject.SetActive(false);
						SetBgState();
					});
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
				UnloadMenuFromTop();
				SetBgState();
			}
			else
			{
				if (menu.AllowPop)
				{
					menu.OnExit();
					_menuCount--;
				}

				menu.transform.DOScale(0, 0.1f).From(1).SetEase(Ease.InBack)
					.OnComplete(() =>
					{
						menu.gameObject.SetActive(false);
						SetBgState();
					});
			}
		}

		// Unloads Last Loaded Menu
		private void UnloadMenuFromTop()
		{
			var count = _serializedMenuCache.Count;

			if (count > 0)
			{
				var targetMenu = _serializedMenuCache[count - 1];
				if (targetMenu.AllowPop) targetMenu.OnExit();
				_menuCount--;
			}
		}

		private void SetBgState()
		{
			_backgroundBlocker.gameObject.SetActive(_menuCount != 0);
		}

		private void SetBgState(bool state)
		{
			_backgroundBlocker.gameObject.SetActive(state);
		}
	}
}
