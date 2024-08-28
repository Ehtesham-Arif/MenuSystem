using Systems.Menu.Runtime;
using Systems.Widgets.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.Menu.Example.Runtime
{
	/// <summary>
	/// Menu refs are to be provided in a separate class
	/// </summary>
	public sealed class MockMenuRefs : MenuRefs
	{
		[SerializeField] private TMP_Text _title;
		[SerializeField] private UxButton _closeBtn;

		public TMP_Text Title => _title;

		public UxButton CloseBtn => _closeBtn;
	}
}
