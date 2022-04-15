using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BoolEvent : UnityEvent<bool>
{

}
public class BoolEventListener : MonoBehaviour
{
	[SerializeField] private BoolEventChannelSO _channel = default;

	public BoolEvent onEventRaised;

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

	private void Respond(bool value)
	{
		if (onEventRaised != null)
			onEventRaised.Invoke(value);
	}
}
