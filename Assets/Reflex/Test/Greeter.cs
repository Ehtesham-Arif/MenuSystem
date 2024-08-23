using System.Collections.Generic;
using Reflex.Attributes;
using UnityEngine;

namespace Reflex.Test
{
	public sealed class Greeter : MonoBehaviour
	{
		[Inject] private readonly IEnumerable<string> _strings = default;

		private void Awake() => Debug.Log(string.Join(" ", _strings));
	}
}
