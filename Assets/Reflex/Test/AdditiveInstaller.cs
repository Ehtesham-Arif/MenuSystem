using Cysharp.Threading.Tasks;
using Reflex.Core;
using Reflex.Extensions;
using UnityEngine;

namespace Reflex.Test
{
	public sealed class AdditiveInstaller : MonoBehaviour, IInstaller
	{
		[SerializeField] private AttInjectExample _example = default;
		[SerializeField] private MenuSystem _menuSystem = default;

		public void InstallBindings(ContainerBuilder containerBuilder)
		{
			containerBuilder.AddSingleton(true);
			containerBuilder.AddSingleton(_menuSystem);

			LoadObj1().Forget();
		}

		private async UniTaskVoid LoadObj1()
		{
			var operation = InstantiateAsync<AttInjectExample>(_example, this.transform);

			await operation;

			foreach (var attInjectExample in operation.Result)
			{
				var sceneContainer = this.gameObject.scene.GetSceneContainer();
				var menuSystem = sceneContainer.Resolve<MenuSystem>();

				attInjectExample.Load(menuSystem);
			}
		}
	}
}
