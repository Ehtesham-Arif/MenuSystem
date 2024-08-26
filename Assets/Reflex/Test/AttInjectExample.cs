using UnityEngine;

namespace Reflex.Test
{
	public sealed class AttInjectExample : MonoBehaviour
	{
		public void Load(MenuSystem menuSystem)
		{
			menuSystem.LoadMenu();
		}

	}
}
