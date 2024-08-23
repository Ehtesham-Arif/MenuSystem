using Reflex.Core;
using UnityEngine;

namespace Reflex.Test
{
	public sealed class ProjectInstaller : MonoBehaviour, IInstaller
	{
		public void InstallBindings(ContainerBuilder builder) => builder.AddSingleton("Hello");
	}
}
