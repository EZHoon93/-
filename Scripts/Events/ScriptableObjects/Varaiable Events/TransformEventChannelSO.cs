
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Transform Event Channel")]
public class TransformEventChannelSO : DescriptionBaseSO
{
    public UnityAction<Transform> onEventRaised;

    public void RaiseEvent(Transform target)
    {
        if (onEventRaised != null)
            onEventRaised.Invoke(target);
    }
}
