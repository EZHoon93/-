using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


//[CreateAssetMenu(menuName = "EachEvents/ChangeHideObject")]

public class EachEventChannelBaseSO<T> : DescriptionBaseSO
{
	private Dictionary<int,  UnityAction<T>> _onEventRaisedDic = new Dictionary<int, UnityAction<T>>();

    public event UnityAction<T> onEventRaised
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

    public void RaiseEvent(int viewID , T action)
    {
        if (_onEventRaisedDic.ContainsKey(viewID))
        {
            _onEventRaisedDic[viewID]?.Invoke(action);
        }
        if (_onEventRaisedDic.ContainsKey(0))
        {
            _onEventRaisedDic[0]?.Invoke(action);
        }
    }

    private void RegisterListener(UnityAction<T> unityAction)
    {
        var viewID = GetViewID(unityAction);
        RegisterListener(viewID, unityAction);
    }
    private void RegisterListener(int viewID , UnityAction<T> unityAction)
    {
        if (_onEventRaisedDic.ContainsKey(viewID))
        {
            _onEventRaisedDic[viewID] += unityAction;
        }
        else
        {
            UnityAction<T> unityEvent = delegate { };
            _onEventRaisedDic.Add(viewID, unityEvent);
            _onEventRaisedDic[viewID] += unityAction;
        }
    }
    private void UnRegisterListener(UnityAction<T> unityAction)
    {
        var viewID = GetViewID(unityAction);
        UnRegisterListener(viewID, unityAction);
    }
    private void UnRegisterListener(int viewID, UnityAction<T> unityAction)
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


    int GetViewID(UnityAction<T> unityAction)
    {
        var monoBehaviourPun = unityAction.Target as MonoBehaviourPun;
        int viewID = 0;

        if (monoBehaviourPun == null)
        {
            var task = unityAction.Target as Task;
            if(task != null)
                monoBehaviourPun = task.Owner.GetComponent<MonoBehaviourPun>();
        }
        if (monoBehaviourPun == null)
        {
            return viewID;
        }
        var photonViewObject = monoBehaviourPun.photonView;
        //플레이어 오브젝트라면
        if (photonViewObject.CompareTag("AI") || photonViewObject.CompareTag("Player"))
        {
            viewID = photonViewObject.ViewID;
        }
        //따로 포톤으로 생성된 오브젝트라면,
        else
        {
            if (photonViewObject.InstantiationData != null)
            {
                var isInt = photonViewObject.InstantiationData[0] is int;
                if (isInt)
                    viewID = (int)photonViewObject.InstantiationData[0];
            }
        }
        return viewID;
    }
}
