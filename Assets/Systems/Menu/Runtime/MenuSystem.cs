using System.IO;
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

		private readonly List<Menu> _menuCache = new();
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
				var menuPath = $"{MENU_LOAD_PATH}/{assetName}.prefab";
				var opHandle = Addressables.LoadAssetAsync<GameObject>(menuPath);

				await opHandle;

				if (opHandle.Status == AsyncOperationStatus.Succeeded)
				{
					var menuObj = Instantiate(opHandle.Result, _menuRoot);
					var menu = menuObj.GetComponent<Menu>();

					menuObj.SetActive(false);

					menu.OnInject(_injectionContainer);

					if (menu is IMenuDataReceiver<TData> dataReceiver)
					{
						dataReceiver.SetData(data);
					}

					menu.OnEnter();
					_menuCache.Add(menu);

					menuObj.SetActive(true);

					menu.transform.DOScale(1, 0.5f).From(0).SetEase(Ease.InBack);
				}
			}
			finally
			{
				_semaphore.Release();
			}
		}

		/// <summary>
		/// Unloads Menu with provided assetKey
		/// </summary>
		public void UnLoadMenu(string assetKey = null)
		{
			if (assetKey == default)
			{
				var count = _menuCache.Count;
				if (count > 0) _menuCache.RemoveAt(count - 1);
			}
			else
			{
				var menu = _menuCache.FirstOrDefault(x => x.AssetKey == assetKey);
				if (menu == default) return;

				menu.OnExit();
				_menuCache.Remove(menu);

				menu.transform.DOScale(0, 0.5f).From(1).SetEase(Ease.InBack)
					.OnComplete(() => { menu.gameObject.SetActive(false); });
			}
		}
	}
}
