using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class objectEventListener : MonoBehaviour
{
	[SerializeField] private objectEventChannelSO _channel = default;

	public UnityEvent<object> onEventRaised;

	private void OnEnable()
	{
		if (_channel != null)
			_channel.onEventRaised += Respond;
	}

	private void OnDisable()
	{
		if (_channel != null)
			_channel.onEventRaised -= Respond;
	}

	private void Respond(object data)
	{
		if (onEventRaised != null)
			onEventRaised.Invoke(data);
	}
}
