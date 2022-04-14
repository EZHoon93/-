using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(menuName = "Events/Effect Event Channel")]
public class EffectEvnetChannelSO : DescriptionBaseSO
{
    public UnityAction<GameObject , Vector3, bool > onEventRaised;

    public void RaiseEvent(GameObject gameObject, Vector3 vector3, bool isRPC = false)
    {
        if (onEventRaised != null)
            onEventRaised.Invoke(gameObject,vector3, isRPC);
    }
}
