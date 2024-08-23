using System.Collections.Generic;
using Reflex.Attributes;
using UnityEngine;

namespace Reflex.Test
{
	public sealed class Additive : MonoBehaviour
	{
		[Inject] private readonly IEnumerable<string> _strings = default;
		[Inject] private readonly bool _bool = default;

		private void Awake()
		{
			Debug.Log(string.Join(" ", _strings));
			Debug.Log(_bool);
		}
	}
}
