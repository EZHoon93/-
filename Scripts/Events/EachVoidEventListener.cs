using System.Collections;
using System.Collections.Generic;

using Photon.Pun;

using UnityEngine;
using UnityEngine.Events;

public class EachVoidEventListener : MonoBehaviourPun
{
    [SerializeField] private EachVoidEventChannelSO _eachVoidEventChannelSO;

    public UnityEvent OnResponse;
    private void OnEnable()
    {
        _eachVoidEventChannelSO.onEventRaised +=Respond;
    }

    private void OnDisable()
    {
        _eachVoidEventChannelSO.onEventRaised -= Respond;
    }

    private void Respond()
    {
        OnResponse?.Invoke();
    }
}
