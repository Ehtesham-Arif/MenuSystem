using Reflex.Core;
using Systems.Menu.Runtime;
using UnityEngine;

namespace Systems.Menu.Test
{
	public sealed class MenuTestInstaller : MonoBehaviour, IInstaller
	{
		[SerializeField] private MenuSystem _menuSystem = default;

		public void InstallBindings(ContainerBuilder containerBuilder)
		{
			containerBuilder.AddSingleton(_menuSystem);
			_menuSystem.OnInject(containerBuilder.Build());
		}
	}
}
