using Reflex.Core;
using UnityEngine;

namespace Reflex.Test
{
	public sealed class GreetInstaller : MonoBehaviour, IInstaller
	{
		public void InstallBindings(ContainerBuilder containerBuilder)
		{
			containerBuilder.AddSingleton("World");
		}
	}
}
