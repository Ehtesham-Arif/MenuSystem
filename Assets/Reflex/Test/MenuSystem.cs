using UnityEngine;

namespace Reflex.Test
{
	public sealed class MenuSystem : MonoSystem
	{
		public void LoadMenu()
		{
			Debug.LogError("xyz");
		}
	}

	public interface ISystem
	{
	}

	public abstract class MonoSystem : MonoBehaviour, ISystem
	{
	}
}
