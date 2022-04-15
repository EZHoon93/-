
using UnityEngine;
using System.Collections;

public class LoginScene : MonoBehaviour  
{
    [Header("Varaibles")]
    [SerializeField] private GameSceneSO _lobbySceneSO;
    [SerializeField] private PhotonStateVaraibleSO _photonStateVaraible;
    [Header("BroadCasting")]
    [SerializeField] SceneLoadEventChannelSO _goToLobbyScene;  //다음씬
    [SerializeField] VoidEventChannelSO _onConnectEvent;


    private IEnumerator Start()
    {
        _photonStateVaraible.Value = PhotonState.DisConnect;
        //PhotonHashConfig.LocalPlayerIsGameJoin = false;
        //PhotonHashConfig.SetLocalPlayerLevel(0);
        yield return new WaitForSeconds(1.0f);
        _onConnectEvent.RaiseEvent();   //연결 시도
        yield return new WaitUntil(Check);
        _goToLobbyScene.RaisedEvent(_lobbySceneSO);
    }
    bool Check()
    {
        return _photonStateVaraible.Value == PhotonState.Connect ? true : false;
    }
}
