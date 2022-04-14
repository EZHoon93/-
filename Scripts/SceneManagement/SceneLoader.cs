

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Photon.Pun;
using Photon.Realtime; // 포톤 서비스 관련 라이브러리
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SceneLoader : MonoBehaviourPunCallbacks
{
    [Header("Varaibles")]
    [SerializeField] private GameModeSO _gameModeSO;
    [SerializeField] private GameStateSO _gameStateSO;
    [SerializeField] private GameSceneContainerSO _sceneContainerSO;
    [SerializeField] private GameSceneSO _lobbySceneSO;
    [SerializeField] private SelectRoomInfo _selectRoomInfo;
 
    [Header("Listening to")]
	[SerializeField] private SceneLoadEventChannelSO _onLoadSceneEventSO;   //씬호출 
    [SerializeField] private VoidEventChannelSO _onConfirmGameRoomExit;    //포톤 게임 나가기 시 이벤트
    [SerializeField] private VoidEventChannelSO _onNextRandomGaemSceneEvent;    //게임 끝나고 다시 랜덤맵 호출
    [SerializeField] private VoidEventChannelSO _onConfirmGameSearchEvent;    //게임 끝나고 다시 랜덤맵 호출
    [SerializeField] private OnChangeRoomPropertiesChannelEventSO _onChangeRoomPropertiesChannelEventSO;    //룸 업데이트
    [Header("BroadCasting ")]
    [SerializeField] private BoolEventChannelSO _setActiveLoadingUI;    //로딩 UI
    [SerializeField] private BoolEventChannelSO _onLoadSceneComplete;    //Scene Start , Game = true, NotGame = false
    [SerializeField] private SceneLoadEventChannelSO _loadSceneEventSO;   //게임 씬 호출 
    [SerializeField] private VoidEventChannelSO _onClearEventSO;
    //[SerializeField] private SceneLoadEventChannelSO _loadNoPhotonScene;   //게임 씬 호출 


    public GameMode CurrentGameMode => _gameModeSO.CurrentMode;


    public override void OnEnable()
    {
        base.OnEnable();
        _onLoadSceneEventSO.OnLoadingRequested += OnLoadScene;
        _onConfirmGameRoomExit.onEventRaised += PhotonGamrRoomExit;
        _onNextRandomGaemSceneEvent.onEventRaised += OnNextRandomGameScene;
        _onConfirmGameSearchEvent.onEventRaised += OnGameSearch;
        _onChangeRoomPropertiesChannelEventSO.onEventRaised += OnChangeRoomProperties;

    }
    public override void OnDisable()
    {
        base.OnDisable();
        _onLoadSceneEventSO.OnLoadingRequested -= OnLoadScene;
        _onConfirmGameRoomExit.onEventRaised -= PhotonGamrRoomExit;
        _onNextRandomGaemSceneEvent.onEventRaised -= OnNextRandomGameScene;
        _onConfirmGameSearchEvent.onEventRaised -= OnGameSearch;
        _onChangeRoomPropertiesChannelEventSO.onEventRaised -= OnChangeRoomProperties;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            OnNextRandomGameScene();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            var ddd = PhotonNetwork.CurrentRoom.CustomProperties;
            foreach (var d in ddd)
            {
                print(d.Key + "/" + d.Value);
            }
        }

    }

    #region CallBack

    private void OnGameSearch()
    {
        var roomName = _selectRoomInfo.roomName;
        var gameMode = _selectRoomInfo.gameMode;
        var maxPlayerCount = _gameModeSO.GetMaxPlayers(_selectRoomInfo.gameMode);

        if (string.IsNullOrEmpty(roomName))
        {
            PhotonNetwork.JoinRandomRoom(new Hashtable { { "gm", (int)gameMode } }, maxPlayerCount);
        }
        //방이름있음 => 수동참여
        else
        {
            PhotonNetwork.JoinOrCreateRoom(roomName, CreateRoom(), TypedLobby.Default);
        }
    }
    private void PhotonGamrRoomExit()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void OnNextRandomGameScene()
    {
        if (PhotonNetwork.IsMasterClient == false)
            return;
        var selectSceneSO = _sceneContainerSO.GetRandomGameScene(CurrentGameMode, null);
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { "mp", selectSceneSO.gameSceneName } });
    }

    private void OnLoadScene(GameSceneSO gameSceneSO)
    {
        _onClearEventSO.RaiseEvent();   //클리어 
        _setActiveLoadingUI.RaiseEvent(true);
        _gameStateSO.SetupGameScene(gameSceneSO);
        if(gameSceneSO.sceneType == GameSceneType.GamePlay)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.DestroyAll();
            }
            PhotonNetwork.LoadLevel(gameSceneSO.gameSceneName);
            StartCoroutine(LoadSceneCorutine());
            IEnumerator LoadSceneCorutine()
            {
                yield return new WaitUntil(() => PhotonNetwork.LevelLoadingProgress == 1);
                _onLoadSceneComplete.RaiseEvent(true);
            }
        }
        else
        {
            StartCoroutine(LoadSceneCorutine());
            IEnumerator LoadSceneCorutine()
            {
                AsyncOperation operation = SceneManager.LoadSceneAsync(gameSceneSO.gameSceneName);
                yield return new WaitUntil(() => operation.isDone);
               _onLoadSceneComplete.RaiseEvent(false);
               _setActiveLoadingUI.RaiseEvent(false);
            }
        }
    }
   

    private void OnChangeRoomProperties(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("mp"))
        {

            var gameSceneName = (string)propertiesThatChanged["mp"];
            LoadGamePlayScene( gameSceneName);
        }
    }

 
    /// <summary>
    /// 참가한 룸 정보로 변경
    /// </summary>
    public override void OnJoinedRoom()
    {
        var HT = PhotonNetwork.CurrentRoom.CustomProperties;
        var n_gameMode = (GameMode)HT["gm"];
        _gameModeSO.Setup(n_gameMode);
        if (PhotonNetwork.IsMasterClient)
        {
            OnNextRandomGameScene();
        }
        else
        {
            var gameSceneName = (string)HT["mp"];
            LoadGamePlayScene(gameSceneName);
        }
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.DestroyAll(true);
        _loadSceneEventSO.RaisedEvent(_lobbySceneSO);
    }

    /// <summary>
    /// 방 참가 실패시 , 아무 방도 없을떄 => 방을 만듬
    /// </summary>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        var roomName = Random.Range(0,999).ToString();
        PhotonNetwork.JoinOrCreateRoom(roomName, CreateRoom(), TypedLobby.Default);

    }
    #endregion

    #region public


    #endregion

    #region private
    private RoomOptions CreateRoom()
    {
        var result = new RoomOptions()
        {
            IsVisible = !_selectRoomInfo.isSecret,
            MaxPlayers = _gameModeSO.GetMaxPlayers(_selectRoomInfo.gameMode),
            CustomRoomPropertiesForLobby = new string[] { "gm" },
            CustomRoomProperties = new Hashtable() 
            {
                    {"gm", (int)_selectRoomInfo.gameMode}  ,       //게임모드
                    //{ "mp", GetRandomGameSceneCode() }    ,   //맵 코드
                    //{"gs", Define.GameState.Wait }  ,   //현재 게임 상태 Wait
                    //{"st", (int)PhotonNetwork.Time }    //상태가 생성된 시간
            }
        };

        return result;
    }
    private void LoadGamePlayScene(string gameSceneName)
    {
        var gameSceneSO = _sceneContainerSO.GetGameSceneSO(CurrentGameMode, gameSceneName);
        _loadSceneEventSO.RaisedEvent(gameSceneSO);
    }

    #endregion

}
