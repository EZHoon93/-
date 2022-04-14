using Photon.Pun;
using UnityEngine.Events;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Data;


/// <summary>
/// 사용자에게 귀속되는 룸오브젝트
/// </summary>

public class PhotonRoomObjectController : MonoBehaviourPun, IPunInstantiateMagicCallback, IOnPhotonViewPreNetDestroy
{
    //[SerializeField] PhotonRoomObjectController[] _subPhotonRoomObjectControllers;

    public PhotonInstantiateEvent onPhotonInstantiateEvent;
    public PhotonDestoryEvent onPhotonDestroyEvent;

    [ContextMenu("Setup")]
    public void Setup()
    {
        //_subPhotonRoomObjectControllers = this.GetComponentsChildrenRemoveMy<PhotonRoomObjectController>();
    }

    
    
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        var infoData = info.photonView.InstantiationData;
        if (infoData == null)
        {
            return;
        }
        var controllerNr = (int)infoData[0];
        SetupAIAndUser(controllerNr);   //초깃값이 User인지, AI인지 구분
        CheckPlayerViewID(info);    //해당 오브젝트가 vid의 값을 갖고잇는지.
        OnPhotonInit(info);
        //var onphotonList = GetComponents<IOnPhotonInstantiate>();
        //if (onphotonList.Length > 0)
        //{
        //    foreach (var o in onphotonList)
        //    {
        //        o.OnMyPhotonInstantiate(info);
        //    }
        //}
        onPhotonInstantiateEvent?.Invoke(info);
        ChildOnPhotonInstantiate(info);
    }

    void ChildOnPhotonInstantiate(PhotonMessageInfo info)
    {
        for(int i =0; i < this.transform.childCount; i++)
        {
            var instantEvent = this.transform.GetChild(i).GetComponent<IPunInstantiateMagicCallback>();
            if(instantEvent != null)
            {
                instantEvent.OnPhotonInstantiate(info);
            }
        }
    }

    protected virtual void OnPhotonInit(PhotonMessageInfo info)
    {

    }
    public void OnPreNetDestroy(PhotonView rootView)
    {
        OnPhotonDestroy(rootView);
        onPhotonDestroyEvent?.Invoke(rootView);
    }

    protected virtual void OnPhotonDestroy(PhotonView rootView)
    {

    }

    void SetupAIAndUser(int controllerNr)
    {
        if (controllerNr > 0 && PhotonNetwork.CurrentRoom.Players.ContainsKey(controllerNr))
        {
            this.gameObject.tag = "User";
            ChangeControllerNr(controllerNr);
        }
        else
        {
            this.gameObject.tag = "AI";
            ChangeControllerNr(PhotonNetwork.MasterClient.ActorNumber);
        }
    }

    /// <summary>
    /// 플레이어 캐릭이라면, 해당 photonView의 값을 "vid"키에 추가.
    /// </summary>
    void CheckPlayerViewID(PhotonMessageInfo info)
    {
        var HT = (Hashtable)info.photonView.InstantiationData[1];
        //Debug.Assert(HT == null, "Error " +this.gameObject.name +"/" +info.photonView.InstantiationData[1].GetType());
        if (HT.ContainsKey("vid") == false)
        {
            HT.Add("vid", info.photonView.ViewID);
        }
    }

    void ChangeControllerNr(int controllerNr)
    {
        if (controllerNr == this.photonView.ControllerActorNr)
        {
            return;
        }
        this.photonView.ControllerActorNr = controllerNr;
    }

    public void AddEvent(UnityAction<PhotonMessageInfo> instantiateAction, UnityAction<PhotonView> destroyAction)
    {
        //onPhotonInstantiateEvent.AddListener(instantiateAction);
        //onPhotonDestroyEvent.AddListener(destroyAction);
    }





}
