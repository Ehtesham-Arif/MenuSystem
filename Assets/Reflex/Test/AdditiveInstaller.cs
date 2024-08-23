using Reflex.Core;
using UnityEngine;

namespace Reflex.Test
{
	public sealed class AdditiveInstaller : MonoBehaviour, IInstaller
	{
		public void InstallBindings(ContainerBuilder containerBuilder)
		{
			containerBuilder.AddSingleton(true);
		}
	}
}
