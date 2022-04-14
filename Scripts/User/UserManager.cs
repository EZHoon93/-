
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
    [SerializeField] private VoidEventChannelSO _onClickGameEnterButtonEventSO; //���� ������ư Ŭ��
    [SerializeField] private VoidEventChannelSO _onClickGameExitButtonEventSO;  //���� ������ ��ư Ŭ��
    [SerializeField] private OnPlayerPropertiesUpdateChannelEventSO _onUpdatePlayerProperties;
    [Header("Broad Casting")]
    [SerializeField] private CheckPopMessageEventCannelSO _checkMessageChannelSO;   //Ȯ�� �˾� â
    [SerializeField] private PhotonEventChannelSO _playerControllerChangeEventChannelSO; //�ڱ��ڽ��� ĳ���� AI�� ����,AI�� �ڽ�ĳ������, �����Ǻ���� ȣ��
    [SerializeField] private BoolEventChannelSO _onLocalPlayerIsGameJoinEventSO;    //�������κ��� �ݹ��� ����
    [SerializeField] private IntEventChannelSO _gameJoinPlayersCountEventSO;    //���� �������� ���� ����
    [SerializeField] private EachVoidEventChannelSO _onControllerChangeToAIEvent;  //��Ʈ�ѷ� ü����.

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
    //    //SendLocalPlayerGameJoin();//������ �������� ������
    //    //SendCurrentGameJoinPlayersCount();//���� ������������ ����
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

        SendCurrentGameJoinPlayersCount();  //���� ������ ������ ī��Ʈ �̺�Ʈ
        if (targetPlayer.IsLocal)
        {
            SendLocalPlayerGameJoin();  //���� �������� ���� ���� �̺�Ʈ �߻�
            
        }
    }


    /// <summary>
    /// ������ AI, User ��ȯ ĳ��  2��°���ڴ� true�Ͻ� AI�� false�Ͻ� ������.
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
        _onLocalPlayerIsGameJoinEventSO.RaiseEvent(isLocalPlayerJoin);  //������ �������� ������
    }

    private void SendCurrentGameJoinPlayersCount()
    {
        var count = UtilPhoton.GetGameReadyPlayersCount();
        _gameJoinPlayersCountEventSO.RaiseEvent(count);  //���� ������������ ����

    }
}
