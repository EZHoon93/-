

using ExitGames.Client.Photon;

using UnityEngine;
using UnityEngine.Events;
using Photon.Realtime;

[CreateAssetMenu(menuName = "Events/PunCallbacks/OnPlayer PropertiesUpdate ChannelEvent")]
public class OnPlayerPropertiesUpdateChannelEventSO : DescriptionBaseSO
{
    public UnityAction<Player ,Hashtable> onEventRaised;
    public void RaiseEvent(Player targetPlayer, Hashtable propertiesThatChanged)
    {
        onEventRaised?.Invoke(targetPlayer, propertiesThatChanged);
    }


}
