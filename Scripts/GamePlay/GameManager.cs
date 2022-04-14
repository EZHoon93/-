
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPun
{
    #region Fields
    [Header("Varaibles")]
    [SerializeField] private GameStateSO _gameStateSO;
    [SerializeField] private PhotonGamingPlayersInfoSO _photonGamingPlayersInfoSO;

    [Header("Listening to")]
    [SerializeField] private IntEventChannelSO _onTeamPlayersSpawnEvent;
    [SerializeField] private SceneLoadEventChannelSO _loadGameScene;   //게임 씬 호출 

    #endregion
    #region Properties



    #endregion

    #region Life Cycle
    private void Awake()
    {
        //_onStartGamePlayEventSO.onEventRaised += OnStartGamePlay;
        //_loadGameScene.OnLoadingRequested += OnLoadScene;
    }

    private void OnDestroy()
    {
        //_onStartGamePlayEventSO.onEventRaised -= OnStartGamePlay;
        //_loadGameScene.OnLoadingRequested -= OnLoadScene;

    }
    #endregion


    #region public

    #endregion


    #region private

    private void OnSpawnTeamPlayers(int teamCode)
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            return;
        }


    }

    private void OnStartGamePlay()
    {

    }
    private void OnLoadScene(GameSceneSO gameSceneSO)
    {
        if(gameSceneSO.sceneType == GameSceneType.GamePlay)
        {
        }
        else
        {
        }
    }

    #endregion
    #region Override, Interface

    #endregion

}
