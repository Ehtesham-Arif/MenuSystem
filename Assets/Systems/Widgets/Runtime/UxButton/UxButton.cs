using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Systems.Widgets.Runtime
{
	public sealed class UxButton : Button, IUxElement
	{
		private readonly List<UnityAction> _subscribedActions = new();

		public void SubscribeClick(UnityAction action)
		{
			onClick.AddListener(action);

			_subscribedActions.Add(action);
		}

		public void Dispose()
		{
			foreach (var unityAction in _subscribedActions)
			{
				onClick.RemoveListener(unityAction);
			}

			_subscribedActions.Clear();
		}
	}
}
