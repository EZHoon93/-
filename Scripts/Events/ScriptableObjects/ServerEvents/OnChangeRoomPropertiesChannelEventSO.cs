
using ExitGames.Client.Photon;

using Photon.Pun;

using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/PunCallbacks/OnChangeRoomProperties Event")]
public class OnChangeRoomPropertiesChannelEventSO : DescriptionBaseSO
{
    public UnityAction<Hashtable> onEventRaised;
    public void RaiseEvent(Hashtable propertiesThatChanged)
    {
        onEventRaised?.Invoke(propertiesThatChanged);
    }
}
