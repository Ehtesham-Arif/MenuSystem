using Reflex.Core;
using Reflex.Runtime;
using UnityEngine;

namespace Systems.Menu.Runtime
{
	public abstract class Menu : MonoBehaviour, IInjectable
	{
		// Injected Menu System
		private MenuSystem _menuSystem = default;

		// Unique Name of each Menu. 
		public string AssetKey { get; set; } = default;

		// Injection of Reflex Container
		public virtual void OnInject(Container container)
		{
			_menuSystem = container.Resolve<MenuSystem>();
		}

		// This property is used when this menu is about to be Loaded out.<br/>
		// True : This menu will not be Unloaded even if Unload Api is invoked over this Menu
		public bool AllowPop { get; set; } = true;

		// Menu Entry
		public abstract void OnEnter();

		// Menu Exit
		public abstract void OnExit();

		// Invoke this api from inside Menu to Unload it
		protected void UnloadMenu() => _menuSystem.UnLoadMenu(this);
	}

	public abstract class Menu<TRefs> : Menu where TRefs : MenuRefs
	{
		[SerializeField] private TRefs _menuRefs;

		protected TRefs MenuRefs => _menuRefs;
	}

	public abstract class Menu<TData, TRefs> : Menu<TRefs>, IMenuDataReceiver<TData>
		where TData : struct, IMenuData
		where TRefs : MenuRefs
	{
		protected TData MenuData = default;

		public void SetData(TData data) => MenuData = data;
	}

	public abstract class MenuRefs : MonoBehaviour
	{
	}
}
