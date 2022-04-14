

using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/UI/PopMessage")]

public class CheckPopMessageEventCannelSO : DescriptionBaseSO
{
    public delegate void CheckPopMessage(string content, UnityAction confirmEvent);

    public CheckPopMessage onCheckPopMessage;

    public void OnPopRaisedEvent(string content, UnityAction confirmAction)
    {
        onCheckPopMessage?.Invoke(content ,confirmAction);
    }
}
