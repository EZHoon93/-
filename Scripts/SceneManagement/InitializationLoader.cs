using System.Collections;

using UnityEngine;

public class InitializationLoader : MonoBehaviour
{
    [Header("Varaibles")]
    [SerializeField] private GameSceneSO _loginSceneSO;
    [SerializeField] private PhotonStateVaraibleSO _photonStateVaraible;

    [Header("BroadCasting")]
    [SerializeField] VoidEventChannelSO _onConnectEvent;
    [SerializeField] SceneLoadEventChannelSO _goToLobbyScene;  //다음씬

    private IEnumerator Start()
    {
        _photonStateVaraible.Value = PhotonState.DisConnect;
        //PhotonHashConfig.LocalPlayerIsGameJoin = false;
        //PhotonHashConfig.SetLocalPlayerLevel(0);
        yield return new WaitForSeconds(1.0f);
        _onConnectEvent.RaiseEvent();   //연결 시도
        yield return new WaitUntil(Check);
        _goToLobbyScene.RaisedEvent(_loginSceneSO);
    }
    bool Check()
    {
        return _photonStateVaraible.Value == PhotonState.Connect ? true : false;
    }
}
