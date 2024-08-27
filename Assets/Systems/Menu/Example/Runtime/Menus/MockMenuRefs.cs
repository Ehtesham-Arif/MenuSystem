using Systems.Menu.Runtime;
using TMPro;
using UnityEngine;

namespace Systems.Menu.Example.Runtime
{
	/// <summary>
	/// Menu refs are to be provided in a separate class
	/// </summary>
	public sealed class MockMenuRefs : MenuRefs
	{
		[SerializeField] private TMP_Text _title;

		public TMP_Text Title => _title;
	}
}
