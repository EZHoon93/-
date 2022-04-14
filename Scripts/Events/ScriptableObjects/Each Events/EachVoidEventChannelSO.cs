using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

/// <summary>
/// 0은 글로벌 
/// </summary>
[CreateAssetMenu(menuName = "EachEvents/Void Event") ]
public class EachVoidEventChannelSO : DescriptionBaseSO
{
    private Dictionary<int, UnityAction> _onEventRaisedDic = new Dictionary<int, UnityAction>();

    public event UnityAction onEventRaised
    {
        add
        {
            RegisterListener(value);
        }
        remove
        {
            UnRegisterListener(value);
        }
    }

    public void RaiseEvent(int viewID)
    {
        if (_onEventRaisedDic.ContainsKey(viewID))
        {
            _onEventRaisedDic[viewID]?.Invoke();
        }
        if (_onEventRaisedDic.ContainsKey(0))
        {
            _onEventRaisedDic[0]?.Invoke();
        }
    }

    private void RegisterListener(UnityAction unityAction)
    {
        var viewID = GetViewID(unityAction);
        RegisterListener(viewID, unityAction);
    }
    private void RegisterListener(int viewID, UnityAction unityAction)
    {
        if (_onEventRaisedDic.ContainsKey(viewID))
        {
            _onEventRaisedDic[viewID] += unityAction;
        }
        else
        {
            UnityAction unityEvent = delegate { };
            _onEventRaisedDic.Add(viewID, unityEvent);
            _onEventRaisedDic[viewID] += unityAction;

        }
    }
    private void UnRegisterListener(UnityAction unityAction)
    {
        var viewID = GetViewID(unityAction);
        UnRegisterListener(viewID, unityAction);
    }
    private void UnRegisterListener(int viewID ,UnityAction unityAction)
    {
        if (_onEventRaisedDic.ContainsKey(viewID) == false)
        {
            return;
        }
        _onEventRaisedDic[viewID] -= unityAction;
        if (_onEventRaisedDic[viewID].GetInvocationList().Length == 0)
        {
            _onEventRaisedDic.Remove(viewID);
        }
    }
    private int GetViewID(UnityAction unityAction)
    {
        var punObject = unityAction.Target as MonoBehaviourPun;
        int viewID = 0;
        if (punObject == null)
        {
            return viewID;
        }
        var photonViewObject = punObject.photonView;
        //플레이어 오브젝트라면 
        if (photonViewObject.CompareTag("AI") || photonViewObject.CompareTag("Player"))
        {
            viewID = photonViewObject.ViewID;
        }
        //따로 포톤으로 생성된 오브젝트라면,
        else
        {
            if(photonViewObject.InstantiationData != null)
            {
                var isInt = photonViewObject.InstantiationData[0] is int;
                if(isInt)
                    viewID = (int)photonViewObject.InstantiationData[0];
            }
        }
        return viewID;
    }
}
