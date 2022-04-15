

using BehaviorDesigner.Runtime;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using Photon.Pun;
using System.Linq;

[System.Serializable]
public class GameStateTimeDic : SerializableDictionaryBase<GameState, int> { }
public class GameStateManager : MonoBehaviourPun , IPunObservable
{
    #region Fields
    [Header("Varaibles")]
    [SerializeField] private GameStateSO _gameStateSO;
    [SerializeField] private PlayerRuntimeSet _playerRuntimeSet;
    [SerializeField] private UIPopUpSO _uiGameResultPopSO;
    [Header("Listening")]
    [SerializeField] private BoolEventChannelSO _onLoadCompleteSceneEvent;
    [Header("BroadCasting")]
    [SerializeField] private GameStateTimerEventSO _gameStateTimerEventSO;  //현재게임상태, 남은시간 전송
    [SerializeField] private VoidEventChannelSO _onChangeGameStateEventSO;  //게임 상태 변경시 
    [SerializeField] private IntEventChannelSO _hiderCountEventSO;
    [SerializeField] private IntEventChannelSO _seekerCountEventSO;
    [SerializeField] private PopUpEventSO _popUpEventSO;
    [Header("Timer Set")]
    public GameStateTimeDic _gameStateTimeDic;
    private int _intRemainTime;
    private float _remainTime;
    private int _hiderCount;
    private int _seekerCount;

    #endregion
    #region Properties

    public GameState CurrentState => _gameStateSO.State;
    public int LastUpdateServerTime { get; private set; }
    public float WaitTime { get; private set; }

    public float RemainTime 
    {
        get => _remainTime;
        set
        {
            var intRemainTime = (int)value;
            if (_intRemainTime != intRemainTime )
            {
                _intRemainTime = intRemainTime;
                _gameStateTimerEventSO.RaiseEvent(CurrentState,intRemainTime);
            }
            _remainTime = value;
        }
    }

    public int HiderCount
    {
        set
        {
            if (_hiderCount == value)
                return;
            _hiderCount = value;
            _hiderCountEventSO.RaiseEvent(value);
        }
    }
    public int SeekerCount
    {
        set
        {
            if (_seekerCount == value)
                return;
            _seekerCount = value;
            _seekerCountEventSO.RaiseEvent(value);
        }
    }


    #endregion

    #region Life Cycle
    private void Awake()
    {
    }
    private void OnEnable()
    {
        _onLoadCompleteSceneEvent.onEventRaised += OnLoadCompleteScene;

    }
    private void OnDisable()
    {
        _onLoadCompleteSceneEvent.onEventRaised -= OnLoadCompleteScene;

    }
    private void Start()
    {
        if (PhotonNetwork.IsMasterClient == false)
            return;
        _gameStateSO.UpdateNewGameStateToServer(GameState.Wait);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            var behavior = GetComponent<BehaviorTree>();
            behavior.enabled = !behavior.enabled;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Break();
        }
    }

    #endregion

    #region public
    /// <summary>
    /// 게임상태 서버로부터 전송받음
    /// </summary>
    public void OnCallBackGameState(object data)
    {
        var datas = (object[])data;
        var newGameState = (GameState)datas[0];
        var createServerTime = (int)datas[1];
        LastUpdateServerTime = createServerTime;
        WaitTime = _gameStateTimeDic[newGameState];
        _gameStateSO.UpdateNexGameState(newGameState, createServerTime);
        _onChangeGameStateEventSO.RaiseEvent();

        if (PhotonNetwork.IsMasterClient == false)
            return;
        switch (newGameState)
        {
            case GameState.Gameing:
                UpdateTeamCount();
                break;
            case GameState.End:
                Define.Team winTeam = _hiderCount == 0 ? Define.Team.Seek : Define.Team.Hide;
                photonView.RPC("ReciveWinTeam", RpcTarget.All, winTeam);
                break;
        }
    }
    public void OnPlayerDeath(object data)
    {
        UpdateTeamCount();
    }
    [PunRPC]
    public void ReciveWinTeam(Define.Team winTeam)
    {
        if(_popUpEventSO.RaiseEvent(_uiGameResultPopSO).TryGetComponent(out UIGameResult uIGameResult))
        {
            uIGameResult.Setup(winTeam);
        }
    }

    #endregion


    #region private
    private void OnLoadCompleteScene(bool isGamePlayScene)
    {
        print("OnLoad CoS");
        if(TryGetComponent(out BehaviorTree behaviorTree))
        {
            behaviorTree.enabled = false;
            behaviorTree.enabled = true;
        }
        if (PhotonNetwork.IsMasterClient == false)
            return;
        _gameStateSO.UpdateNewGameStateToServer(GameState.Wait);
    }
    private void UpdateTeamCount()
    {
        HiderCount = _playerRuntimeSet.Items.Count(x => x.Team == Define.Team.Hide && x.IsDead == false);
        SeekerCount = _playerRuntimeSet.Items.Count(x => x.Team == Define.Team.Seek && x.IsDead == false);
    }
    #endregion


    #region Override, Interface

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_hiderCount);
            stream.SendNext(_seekerCount);

        }
        else
        {
            HiderCount = (int)stream.ReceiveNext();
            SeekerCount = (int)stream.ReceiveNext();
        }
    }
    #endregion


}
