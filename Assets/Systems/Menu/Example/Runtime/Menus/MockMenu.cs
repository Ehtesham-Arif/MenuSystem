using Systems.Menu.Runtime;
using UnityEngine;

namespace Systems.Menu.Example.Runtime
{
	/// <summary>
	/// Normally Refs are not shared between two menus. But there is no harm in using same for two different menus
	/// </summary>
	public sealed class MockMenu : Menu<MockMenuRefs>
	{
		public override void OnEnter()
		{
			Debug.Log($"Mock Menu without Data is entered");

			MenuRefs.CloseBtn.SubscribeClick(UnloadMenu);
		}

		public override void OnExit()
		{
			Debug.Log($"Mock Menu without Data is exited");
		}
	}
}
