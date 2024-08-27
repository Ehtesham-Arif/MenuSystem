using Systems.Menu.Runtime;
using UnityEngine;

namespace Systems.Menu.Example.Runtime
{
	public class MockMenuWithData : Menu<MockMenuData, MockMenuRefs>
	{
		public override void OnEnter()
		{
			Debug.Log($"Mock Menu with Data is entered");
			Debug.Log($"MockMenu loaded with Data {nameof(MockMenuData)} with name {MenuData.Name}");

			MenuRefs.Title.SetText(MenuData.Name);
			MenuRefs.CloseBtn.onClick.AddListener(UnloadMenu);
		}

		public override void OnExit()
		{
			Debug.Log($"Mock Menu with Data is Exited");
			MenuRefs.CloseBtn.onClick.RemoveListener(UnloadMenu);
		}
	}
}
