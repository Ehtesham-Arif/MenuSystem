using System;
using Reflex.Core;
using Reflex.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Reflex.Test
{
	public sealed class LoadAdditiveScene : MonoBehaviour
	{
		private void Start()
		{
			LoadNextScene();
		}

		private void LoadNextScene()
		{
			// Scene Manager Sample
			var bootScene = SceneManager.GetSceneByName("Greet");
			var sessionScene = SceneManager.LoadScene("Additive", new LoadSceneParameters(LoadSceneMode.Additive));
			//ReflexSceneManager.OverrideSceneParentContainer(scene: sessionScene, parent: bootScene.GetSceneContainer());

			// Addessable Sample
			// var handle = Addressables.LoadSceneAsync("Session", LoadSceneMode.Additive, activateOnLoad: false);
			// await handle.Task;
			// var bootScene = SceneManager.GetSceneByName("Boot");
			// ReflexSceneManager.OverrideSceneParentContainer(scene: handle.Result.Scene, parent: bootScene.GetSceneContainer());
			// handle.Result.ActivateAsync();
		}
	}
}
