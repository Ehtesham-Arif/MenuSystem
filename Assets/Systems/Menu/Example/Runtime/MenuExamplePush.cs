using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using Systems.Menu.Runtime;
using UnityEngine;

namespace Systems.Menu.Example.Runtime
{
	/// <summary>
	/// Use this script to push mock menus from context menu
	/// </summary>
	public sealed class MenuExamplePush : MonoBehaviour
	{
		[Inject] private readonly MenuSystem _menuSystem = default;

		[ContextMenu("PushMenu")]
		private void SamplePushMenu()
		{
			_menuSystem.LoadMenu(_menuSystem.Names.MockMenu, IMenuData.Default).Forget();
		}

		[ContextMenu("PushMenuWithData")]
		private void SamplePushMenuWithData()
		{
			var data = new MockMenuData("MockMenu");
			_menuSystem.LoadMenu(_menuSystem.Names.MockMenu, data).Forget();
		}
	}
}
