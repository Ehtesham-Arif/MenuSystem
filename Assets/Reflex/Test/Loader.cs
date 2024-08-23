using Reflex.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Reflex.Test
{
	public sealed class Loader : MonoBehaviour
	{
		private void Start()
		{
			// If you are loading scenes without addressables
			var scene = SceneManager.LoadScene("Greet", new LoadSceneParameters(LoadSceneMode.Single));
			ReflexSceneManager.PreInstallScene(scene, builder => builder.AddSingleton("Beautiful"));

			// If you are loading scenes with addressables
			// Addressables.LoadSceneAsync("Greet", activateOnLoad: false).Completed += handle =>
			// {
			// 	ReflexSceneManager.PreInstallScene(handle.Result.Scene, builder => builder.AddSingleton("Beautiful"));
			// 	handle.Result.ActivateAsync();
			// };
		}
	}
}
