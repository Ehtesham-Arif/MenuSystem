using Reflex.Core;
using Reflex.Runtime;
using UnityEngine;

namespace Systems.Menu.Runtime
{
	public abstract class Menu : MonoBehaviour, IInjectable
	{
		private MenuSystem _menuSystem = default;
		
		public string AssetKey => this.gameObject.name;

		public virtual void OnInject(Container container)
		{
			_menuSystem = container.Resolve<MenuSystem>();
		}

		public abstract void OnEnter();

		public abstract void OnExit();

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

	public class MenuRefs : MonoBehaviour
	{
	}
}
