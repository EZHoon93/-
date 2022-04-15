using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MainModeSetup : MonoBehaviourPun  , IPunInstantiateMagicCallback
{
    [SerializeField]
    private PlayerUI _playerUIPrefab;
    private PlayerController _playerController;

    private void Reset()
    {
        print(this.transform.childCount);
    }
    private void Awake()
    {
        _playerController = this.transform.parent.GetComponent<PlayerController>();
    }
    #region CallBack Interface
    ///["nu"] = player.ActorNumber 넘버  ["nn"] = player.NickName 닉네임
    //["te"] = Define.Team.Hide  팀      ["ch"] = HT["ch"],  캐릭스킨
    //["we"] = HT["we"]//무기스킨          ["ac"] = HT["ac"], 악세스킨
    // 우선 AI로 생성.. 자기자신캐릭터는 버퍼를 보냄
    //protected void OnPhotonInstantiate(PhotonMessageInfo info)
    //{
    //    var playerController = GetComponent<PlayerController>();
    //    CraeteUI(playerController, info);
    //}
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (info.photonView.InstantiationData == null)
            return;
            CraeteUI( info);
    }
    void CraeteUI(PhotonMessageInfo info)
    {
        Instantiate(_playerUIPrefab).Setup(_playerController, info);
        //playerUI.Setup(playerController, info);
        //playerController.PlayerUI = playerUI;
        //playerUI.SetActiveGroundUI(false);
    }
    public void OnPhotonDestroy(PhotonView photonView)
    {
    }

   



    #endregion


}
