using Reflex.Core;
using Systems.Menu.Runtime;
using UnityEngine;

namespace Systems.Menu.Example.Runtime
{
	/// <summary>
	/// Using Reflex as a dependency Injector, Installer is a must in scene where injection is required. Only script which
	/// needs to push some menu should get injected Menu System, it should not be available statically
	/// </summary>
	public sealed class MenuExampleInstaller : MonoBehaviour, IInstaller
	{
		[SerializeField] private MenuSystem _menuSystem = default;

		public void InstallBindings(ContainerBuilder containerBuilder)
		{
			containerBuilder.AddSingleton(_menuSystem);
			_menuSystem.OnInject(containerBuilder.Build());
		}
	}
}
