
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Photon/PhotonEvent_")]
public class PhotonEventChannelSO : ScriptableObject 
{
    //public static byte code = 10;
    //[SerializeField] protected abstract  byte _eventCode;
    [SerializeField] DeliveryMode _deliveryMode;
    [SerializeField] EventCaching _eventCaching;
    [SerializeField] bool _isRemovePrevEvent;   //이전데이터 제거여부
    [SerializeField] private byte _eventCode;

    public UnityAction<object> onEventRaised;
    public byte eventCode => _eventCode;


    public void RaiseEventRemoveCached(object key)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            CachingOption = EventCaching.RemoveFromRoomCache,
            Receivers = ReceiverGroup.All,
        };

        
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(eventCode, key, raiseEventOptions, sendOptions);
    }



    // 이벤트 트리거가 옵저버들에게 전파(Notify)하기 위한 함수.
    public void RaiseEventToServer(object data)
    {

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            CachingOption = _eventCaching,
            Receivers = ReceiverGroup.All,
        };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(_eventCode, data, raiseEventOptions, sendOptions);

    }


}
