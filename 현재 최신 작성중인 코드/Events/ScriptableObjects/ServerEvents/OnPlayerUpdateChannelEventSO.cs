
using Photon.Realtime;

using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/PunCallbacks/UpdatePlayer ChannelEvent")]
public class OnPlayerUpdateChannelEventSO : DescriptionBaseSO
{
    //public UnityAction<Player> onEventRaised;
    public UnityEvent<Player> onEventRaised;

    public void RaiseEvent(Player player)
    {
        onEventRaised?.Invoke(player);
    }


}
