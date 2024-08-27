using Reflex.Core;
using Reflex.Runtime;
using UnityEngine;

namespace Systems.Menu.Runtime
{
	public abstract class Menu : MonoBehaviour, IInjectable
	{
		public string AssetKey => this.gameObject.name;

		public virtual void OnInject(Container container)
		{
			// override able on use case dependency
		}

		public abstract void OnEnter();

		public abstract void OnExit();
	}

	public abstract class Menu<TRefs> : Menu where TRefs : MenuRefs
	{
		[SerializeField] private TRefs _menuRefs;

		public TRefs MenuRefs => _menuRefs;
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
