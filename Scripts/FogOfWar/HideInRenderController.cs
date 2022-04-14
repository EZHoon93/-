using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

/// <summary>
/// LivingEntitiy에 등록하는 오브젝,
/// </summary>
public class HideInRenderController : MonoBehaviour
{
    [SerializeField] Canvas _canvas;
    [SerializeField] Renderer[] _renderers;

    //LivingEntity _livingEntity;


    private void Reset()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        _canvas = GetComponentInChildren<Canvas>();
    }
    private void Awake()
    {
        //GetComponent<PhotonRoomObject>().AddEvent(OnPhotonInstantiate, OnPhotonDestroy);
    }
    //public void OnPhotonDestroy(PhotonView photonView)
    //{
    //    if (_livingEntity == null)
    //        return;

    //    //_livingEntity.RemoveRenderer(this);
    //}

    //void OnPhotonInstantiate(PhotonMessageInfo photonMessageInfo)
    //{
    //    var HT = (Hashtable)photonMessageInfo.photonView.InstantiationData[1];
    //    var viewID = (int)HT["vid"];
    //    //var livingEntitiy = Managers.Game.GetLivingEntity(viewID);

    //    Setup(livingEntitiy);
    //}

    //public void Setup(LivingEntity livingEntity)
    //{
    //    if (_livingEntity == null)
    //        return;

    //    //_livingEntity.AddRenderer(this);
    //}

   

  

  
}
