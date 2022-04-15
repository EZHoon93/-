
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
/// <summary>
/// 생서 및 파괴 시 
/// </summary>
//[RequireComponent(typeof(PlayerController))]
//[RequireComponent(typeof(PhotonView))]
public class PlayerSetup : MonoBehaviourPun 
{
    [SerializeField]
    private PhotonView _subPhotonView;
    [SerializeField]
    private PlayerUI _playerUIPrefab;
    private void Awake()
    {
        //GetComponent<PhotonRoomObject>()?.AddEvent(OnPhotonInstantiate, OnPhotonDestroy);
    }
    #region CallBack Interface
    ///["nu"] = player.ActorNumber 넘버  ["nn"] = player.NickName 닉네임
    //["te"] = Define.Team.Hide  팀      ["ch"] = HT["ch"],  캐릭스킨
    //["we"] = HT["we"]//무기스킨          ["ac"] = HT["ac"], 악세스킨
    // 우선 AI로 생성.. 자기자신캐릭터는 버퍼를 보냄
    protected  void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        var playerController = GetComponent<PlayerController>();
        CraeteUI(playerController, info);
        if (this.IsMyCharacter())
        {

        }
        //playerController.PlayerHealth.Team = team;
        //Data.PlayerControllerInfo playerControllerInfo = new Data.PlayerControllerInfo();
        //playerControllerInfo.nickName = nickName;
        //playerControllerInfo.level = 1;
        //playerController.PlayerInfo = playerControllerInfo;       //닉네임 설정
        //CraeteUI(playerController);
    }

    void CraeteUI(PlayerController playerController, PhotonMessageInfo info)
    {
        var playerUI = Instantiate(_playerUIPrefab);
        playerUI.Setup(playerController, info);
        playerController.PlayerUI = playerUI;
    }
    protected void OnPhotonDestroy(PhotonView photonView)
    {
        var playerController = this.GetComponent<PlayerController>();
        //playerController.OnPhotonDestroy(photonView);
    }



    #endregion




}
