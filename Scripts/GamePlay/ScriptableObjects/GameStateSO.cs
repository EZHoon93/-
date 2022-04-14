
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using RotaryHeart.Lib.SerializableDictionary;

public enum GameState
{
    Wait,
    CountDown,
    GameReady,
    Gameing,
    End
}


[System.Serializable]
public class GameStateEventDic : SerializableDictionaryBase<GameState, VoidEventChannelSO> { }

[CreateAssetMenu(menuName = "GamePlay/GameState")]
public class GameStateSO : DescriptionBaseSO
{
    [SerializeField] private GameState _gameState;
    [SerializeField] private GameSceneSO _gameSceneSO;
    [SerializeField] private GamePlaySceneSO _gamePlaySceneSO;

    [Header("BroadCasting")]
    [SerializeField] private PhotonEventChannelSO _photonGameStateEventSO;  //게임상태변경 서버전송
    [SerializeField] private GameStateEventDic _gameStateVoidEventDicSO;


    public GameState State => _gameState;
    public GameSceneSO CurrentGameSceneSO => _gameSceneSO;
    public GamePlaySceneSO CurrentGamePlaySceneSO => _gamePlaySceneSO;

    private void OnEnable()
    {
        _gameState = GameState.Wait;
    }

    public void SetupGameScene(GameSceneSO gameSceneSO)
    {
        _gameSceneSO = gameSceneSO;
        var isGamePlayScene = gameSceneSO is GamePlaySceneSO;
        if (isGamePlayScene)
            _gamePlaySceneSO = gameSceneSO as GamePlaySceneSO;
    }
    //public void Setup
    public void UpdateNexGameState(GameState newGameState, float createTime)
    {
        _gameState = newGameState;
        //이벤트 
        if (_gameStateVoidEventDicSO.TryGetValue(newGameState, out var voidEventSO))
        {
            voidEventSO.RaiseEvent();
        }
    }
    /// <summary>
    /// 다음 게임상태 서버로전송
    /// </summary>
    public void UpdateNewGameStateToServer(GameState newGameState)
    {
        if (!PhotonNetwork.IsMasterClient)  //방장만 실행
            return;
        _photonGameStateEventSO.RaiseEventToServer(new object[] { newGameState, PhotonNetwork.ServerTimestamp });
    }


}
