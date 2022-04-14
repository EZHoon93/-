
using Photon.Pun;
using Photon.Realtime; // 포톤 서비스 관련 라이브러리
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonManager : MonoBehaviourPunCallbacks 
{
    [Header("Varaibles")]
    [SerializeField] private PhotonStateVaraibleSO _photonState;
    [SerializeField] private GameModeSO _gameModeSO;

    [Header("Listening")]
    [SerializeField] private VoidEventChannelSO _onTryConnectServerEventSO;

    [Header("BroadCasting")]
    [SerializeField] private OnChangeRoomPropertiesChannelEventSO _onChangeRoomPropertiesChannelEventSO;
    [SerializeField] private OnPlayerPropertiesUpdateChannelEventSO _onPlayerPropertiesUpdateChannelEventSO;
    [SerializeField] private OnPlayerUpdateChannelEventSO _onPlayerEnterRoomEventSO;
    [SerializeField] private OnPlayerUpdateChannelEventSO _onPlayerLeftRoomEventSO;

    readonly string _gameVersion = "1.0.1";



    public override void OnEnable()
    {
        base.OnEnable();
        _onTryConnectServerEventSO.onEventRaised += Connect;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        _onTryConnectServerEventSO.onEventRaised -= Connect;
    }

    //포톤 서버 연결
    public void Connect()
    {
        _photonState.Value = PhotonState.Connecting;
        PhotonNetwork.GameVersion = _gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        print("Connect");
    }

    /// 포톤 서버 접속시 자동 실행
    public override void OnConnectedToMaster()
    {
        //마스터 서버에 접속중이라면
        if (PhotonNetwork.IsConnected)
        {
            _photonState.Value = PhotonState.Connect;
            // 룸 접속 실행
            PhotonNetwork.ConnectToRegion("kr");
        }
        else
        {
            _photonState.Value = PhotonState.DisConnect;
            //재연결
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public override void OnConnected()
    {
        print("OnConnet !!");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
    }
    #region EventCallBack

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) => _onPlayerPropertiesUpdateChannelEventSO.RaiseEvent(targetPlayer, changedProps);

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) => _onChangeRoomPropertiesChannelEventSO.RaiseEvent(propertiesThatChanged);


    public override void OnPlayerLeftRoom(Player otherPlayer) => _onPlayerLeftRoomEventSO.RaiseEvent(otherPlayer);


    public override void OnPlayerEnteredRoom(Player newPlayer) => _onPlayerEnterRoomEventSO.RaiseEvent(newPlayer);
 
    #endregion

    #region private


    #endregion
}
