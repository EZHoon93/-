

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Photon.Pun;
using Photon.Realtime; // ���� ���� ���� ���̺귯��
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
	[SerializeField] private SceneLoadEventChannelSO _onLoadSceneEventSO;   //��ȣ�� 
    [SerializeField] private VoidEventChannelSO _onConfirmGameRoomExit;    //���� ���� ������ �� �̺�Ʈ
    [SerializeField] private VoidEventChannelSO _onNextRandomGaemSceneEvent;    //���� ������ �ٽ� ������ ȣ��
    [SerializeField] private VoidEventChannelSO _onConfirmGameSearchEvent;    //���� ������ �ٽ� ������ ȣ��
    [SerializeField] private OnChangeRoomPropertiesChannelEventSO _onChangeRoomPropertiesChannelEventSO;    //�� ������Ʈ
    [Header("BroadCasting ")]
    [SerializeField] private BoolEventChannelSO _setActiveLoadingUI;    //�ε� UI
    [SerializeField] private BoolEventChannelSO _onLoadSceneComplete;    //Scene Start , Game = true, NotGame = false
    [SerializeField] private SceneLoadEventChannelSO _loadSceneEventSO;   //���� �� ȣ�� 
    [SerializeField] private VoidEventChannelSO _onClearEventSO;
    //[SerializeField] private SceneLoadEventChannelSO _loadNoPhotonScene;   //���� �� ȣ�� 


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
        //���̸����� => ��������
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
        _onClearEventSO.RaiseEvent();   //Ŭ���� 
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
    /// ������ �� ������ ����
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
    /// �� ���� ���н� , �ƹ� �浵 ������ => ���� ����
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
                    {"gm", (int)_selectRoomInfo.gameMode}  ,       //���Ӹ��
                    //{ "mp", GetRandomGameSceneCode() }    ,   //�� �ڵ�
                    //{"gs", Define.GameState.Wait }  ,   //���� ���� ���� Wait
                    //{"st", (int)PhotonNetwork.Time }    //���°� ������ �ð�
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
