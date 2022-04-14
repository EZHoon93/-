using ExitGames.Client.Photon;

using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using UnityEngine.Events;
public class PhotonEventListener : MonoBehaviour ,IOnEventCallback
{
	[SerializeField] private PhotonEventChannelSO _channel = default;

	public UnityEvent<object> onEventRaised;

	private void OnEnable()
	{
		PhotonNetwork.AddCallbackTarget(this);
	}

	private void OnDisable()
	{
		PhotonNetwork.RemoveCallbackTarget(this);

	}
    public void OnEvent(EventData photonEvent)
    {
		if (photonEvent.Code != _channel.eventCode)
			return;
		print("onEvent");
		onEventRaised?.Invoke(photonEvent.CustomData);

	}
}
