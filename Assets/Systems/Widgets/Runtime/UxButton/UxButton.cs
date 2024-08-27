using UnityEngine.Events;
using UnityEngine.UI;

namespace Systems.Widgets.Runtime
{
	public sealed class UxButton : Button, IUxElement
	{
		private UnityAction _subscribedAction = default;

		public void SubscribeClick(UnityAction action)
		{
			onClick.AddListener(action);

			_subscribedAction = action;
		}

		public void Dispose()
		{
			if (_subscribedAction != default)
			{
				onClick.RemoveListener(_subscribedAction);
				_subscribedAction = default;
			}
		}
	}
}
