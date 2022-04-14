
using System.Collections;

using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class UserManager : MonoBehaviourPun
{
    [Header("Debug")]
    public GameState CurrentGameState;
    //[SerializeField] private GameObject _userControllerPrefab;
    [Header("Varaibles")]
    //[SerializeField] private CurrentGameModeVaraibleSO _currentGameModeSO;
    //[SerializeField] private CurrentGameStateVariableSO _currentGameStateSO;
    //[SerializeField] private CurrentRoomPlayersInfoSO _currentRoomPlayersInfoSO;
    [SerializeField] private PlayerRuntimeSet _playerRuntimeSet;
    [Header("Listening")]
    [SerializeField] private VoidEventChannelSO _onClickGameEnterButtonEventSO; //게임 참가버튼 클릭
    [SerializeField] private VoidEventChannelSO _onClickGameExitButtonEventSO;  //게임 나가기 버튼 클릭
    [SerializeField] private OnPlayerPropertiesUpdateChannelEventSO _onUpdatePlayerProperties;
    [Header("Broad Casting")]
    [SerializeField] private CheckPopMessageEventCannelSO _checkMessageChannelSO;   //확인 팝업 창
    [SerializeField] private PhotonEventChannelSO _playerControllerChangeEventChannelSO; //자기자신의 캐릭을 AI로 변경,AI를 자신캐릭으로, 소유권변경시 호출
    [SerializeField] private BoolEventChannelSO _onLocalPlayerIsGameJoinEventSO;    //포톤으로부터 콜백후 전송
    [SerializeField] private IntEventChannelSO _gameJoinPlayersCountEventSO;    //현재 참여중인 유저 전송
    [SerializeField] private EachVoidEventChannelSO _onControllerChangeToAIEvent;  //컨트롤러 체인지.

    private void OnEnable()
    {
        _onClickGameEnterButtonEventSO.onEventRaised += OnClickConfirmGameEnter;
        _onClickGameExitButtonEventSO.onEventRaised += OnClickConfirmGameExit;
        _onUpdatePlayerProperties.onEventRaised += OnUpdatePlayerProperties;
    }
    private void OnDisable()
    {
        _onClickGameEnterButtonEventSO.onEventRaised -= OnClickConfirmGameEnter;
        _onClickGameExitButtonEventSO.onEventRaised -= OnClickConfirmGameExit;
        _onUpdatePlayerProperties.onEventRaised -= OnUpdatePlayerProperties;
    }
    //private IEnumerator Start()
    //{

    //    //yield return new WaitForSeconds(1.0f);
    //    //SendLocalPlayerGameJoin();//로컬의 참여여부 재전송
    //    //SendCurrentGameJoinPlayersCount();//현재 게임참가유저 전송
    //}
    #region CallBack

    private void OnClickConfirmGameEnter()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "jn", true } });

    }
    private void OnClickConfirmGameExit()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "jn", false } });
        ChangeControllerToServer(true);
    }
    private void OnUpdatePlayerProperties(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("jn") == false)
            return;

        SendCurrentGameJoinPlayersCount();  //현재 참가한 유저수 카운트 이벤트
        if (targetPlayer.IsLocal)
        {
            SendLocalPlayerGameJoin();  //현재 로컬유저 참가 여부 이벤트 발생
            
        }
    }


    /// <summary>
    /// 서버로 AI, User 전환 캐쉬  2번째인자는 true일시 AI로 false일시 유저로.
    /// </summary>
    private void ChangeControllerToServer(bool isUserToAI)
    {
        var myPlayerController = _playerRuntimeSet.GetMyController();
        if (myPlayerController)
        {
            _playerControllerChangeEventChannelSO.RaiseEventToServer(new object[] { myPlayerController.ViewID(), isUserToAI });
        }
    }

    [CallBack]
    public void CallBackChangeController(object data)
    {
        var datas = (object[])data;
        var playerViewID = (int)datas[0];
        var isUserToAI = (bool)datas[1];
        var playerController = _playerRuntimeSet.GetItem(playerViewID);
        if (playerController)
        {
            var tag = isUserToAI ? "AI" : "User";
            playerController.tag = tag;
            if (isUserToAI)
            {
                playerController.ControllerNr = 0;
                _onControllerChangeToAIEvent.RaiseEvent(playerController.ViewID());
            }
        }

    }


    #endregion

    private void SendLocalPlayerGameJoin()
    {
        var isLocalPlayerJoin = UtilPhoton.IsGameJoinLocalPlayer();
        _onLocalPlayerIsGameJoinEventSO.RaiseEvent(isLocalPlayerJoin);  //로컬의 참여여부 재전송
    }

    private void SendCurrentGameJoinPlayersCount()
    {
        var count = UtilPhoton.GetGameReadyPlayersCount();
        _gameJoinPlayersCountEventSO.RaiseEvent(count);  //현재 게임참가유저 전송

    }
}
