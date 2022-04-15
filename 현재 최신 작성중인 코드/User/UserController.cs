
using Photon.Pun;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class UserController : MonoBehaviourPun, IPunInstantiateMagicCallback, IOnPhotonViewPreNetDestroy
{
    [Header("RunTime Set")]
    [SerializeField] private UserRunTimeSet _userRunTimeSet;
    [SerializeField] private PlayerRuntimeSet _playerRuntimeSet;
    [Header("Varaibles")]
    [SerializeField] private BoolVaraibleSO _localUserIsGameJoin;   //게임 참가 여부
    [Header("CallBack Event")]
    //[SerializeField] private BoolEventChannelSO _callBackPhotonMyGameJonSO; //포톤으로 자기자신의 게임참여여부 콜백
    [SerializeField] private BoolEventChannelSO _onClickGameJoinButtonSO;   //UIInGameMeui의 참가버튼 클릭 및 게임 나가기시 
    [Header("Broad Casting")]
    [SerializeField] private PhotonEventChannelSO _playerControllerChangeEventChannelSO; //자기자신의 캐릭을 AI로 변경,AI를 자신캐릭으로, 소유권변경시 호출


    public bool IsJoin => (bool)PhotonNetwork.LocalPlayer.CustomProperties["jn"];
    public string NickName => this.photonView.Controller.NickName;




    private void OnEnable()
    {
        _userRunTimeSet.Add(this);
        _onClickGameJoinButtonSO.onEventRaised           += OnClickGameJoin;    //게임참가및 나가기 UI 클릭시
    }

    private void OnDisable()
    {
        _userRunTimeSet.Remove(this);
        _onClickGameJoinButtonSO.onEventRaised               -= OnClickGameJoin; //게임참가및 나가기 UI 클릭시
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = this;

        if (photonView.IsMine == false)
            return;

        _localUserIsGameJoin.Value = IsJoin;
        //게임 참가여부로 다시 재설정
        //CallBackPhotonEventByJoin(IsJoin);
    }
    public void OnPreNetDestroy(PhotonView rootView)
    {
    }

    /// <summary>
    /// 메뉴에서 게임참가 및 게임나가기 클릭시 
    /// </summary>
    private void OnClickGameJoin(bool join)
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "jn", join } });
        print("서버전송" + join);

        if (join == false)
        {
            ChangeControllerToServer(true);
        }
    }

    /// <summary>
    /// AI로 난입하기..! 임시?
    /// </summary>
    private void OnClickGameJoinByGameing()
    {

    }


    /// <summary>
    /// 서버로 AI, User 전환 캐쉬  2번째인자는 true일시 AI로 false일시 유저로.
    /// </summary>
    void ChangeControllerToServer(bool isUserToAI)
    {
        var myPlayerController = _playerRuntimeSet.GetMyController();
        if (myPlayerController)
        {
            _playerControllerChangeEventChannelSO.RaiseEventToServer(new object[] { myPlayerController.ViewID() , isUserToAI });
        }
    }
}



