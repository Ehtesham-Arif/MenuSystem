using Cysharp.Threading.Tasks;
using NaughtyAttributes;
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

		[Button]
		private void SamplePushMenu()
		{
			_menuSystem.LoadMenu(_menuSystem.Names.MockMenu, IMenuData.Default).Forget();
		}

		[Button]
		private void SamplePushMenuWithData()
		{
			var data = new MockMenuData("MockMenu");
			_menuSystem.LoadMenu(_menuSystem.Names.MockMenuWithData, data).Forget();
		}
	}
}
