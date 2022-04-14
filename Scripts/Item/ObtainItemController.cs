using UnityEngine;
using Photon.Pun;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;

public class ObtainItemController : MonoBehaviourPun, IPunInstantiateMagicCallback , ITriggerController
{
    #region Varaibles
    [Header("Varaiables")]
    [SerializeField] private PlayerRuntimeSet _playerRuntimeSet;
    [Header("Listening")]
    [SerializeField] private IntEventChannelSO _onPlayerDieEvent;
    [Header("BroadCasting")]
    [SerializeField] private IntEventChannelSO _createItemEvent;
    [SerializeField] private IntEventChannelSO _destroyItemEvent;
    [SerializeField] private PhotonEventChannelSO _enterPlayerEvent;
    [Header("Component")]
    [SerializeField] private Transform _modelPivot;
    [SerializeField] private ParticleSystem _onCompleteGetEffect;
    [SerializeField] private Slider _timeSlider;
    [SerializeField] private float _getNeedTime;

    private int _enterServerTime;
    private int _entetPlayerViewID; //현재 얻고있는 플레이어 뷰아이디
    
    //private bool isPlay;
    #endregion
    #region Properties
    #endregion

    #region Life Cycle
    private void OnEnable()
    {
        _onPlayerDieEvent.onEventRaised += OnPlayerControllerDie;
    }
    private void OnDisable()
    {
        _onPlayerDieEvent.onEventRaised -= OnPlayerControllerDie;

        //NotiFy
        var spawnIndex = (int)this.photonView.InstantiationData[0];
        _destroyItemEvent.RaiseEvent(spawnIndex);
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        var spawnIndex = (int)info.photonView.InstantiationData[0];
        _onCompleteGetEffect.Play();
        _timeSlider.value = 0; 
        _timeSlider.maxValue = _getNeedTime;
        _createItemEvent.RaiseEvent(spawnIndex);

    }

    /// <summary>
    /// 파괴시 마스터가 생성 호출
    /// </summary>
    private void OnDestroy()
    {
        if (PhotonNetwork.IsMasterClient == false)
            return;
        var player = _playerRuntimeSet.GetItem(_entetPlayerViewID);
        if (player == null)
            return;
        if(player.TryGetComponent(out Damageable damageable))
        {
            if (damageable.IsDead)
                return;
        }

        _onCompleteGetEffect.Play();
        var obtainItemKey = (string)photonView.InstantiationData[1];
        PhotonNetwork.InstantiateRoomObject(obtainItemKey, Vector3.down, Quaternion.identity, 0, new object[] { _entetPlayerViewID });
    }
    #endregion

    #region public
    #endregion

    #region CallBack

    public void CachedCallBackEnterPlayer(object data)
    {
        var HT = (Hashtable)data;
        if (HT.TryGetValue(PhotonKeyConfig.viewID,out var viewID))
        {
            if(this.photonView.ViewID != (int)viewID)
            {
                return;
            }
        }
        if (HT.TryGetValue("cn",out var value))
        {
            var controllerNr = (int)value;
            this.photonView.ControllerActorNr = controllerNr;
        }
        if (HT.TryGetValue(PhotonKeyConfig.playerViewID, out var enterPlayerViewID))
        {
            _entetPlayerViewID = (int)enterPlayerViewID;
        }
        if (HT.TryGetValue("st",out var severTime))
        {
            _enterServerTime = (int)severTime;
            StopAllCoroutines();
            StartCoroutine(UpdateTime());
        }
    }
    private void OnPlayerControllerDie(int viewID)
    {
        if(_entetPlayerViewID == viewID)
        {
            StopAllCoroutines();
            _entetPlayerViewID = 0;
            _enterServerTime = 0;
        }
    }

    #endregion

    #region private
    public void OnTriggerChangeDetected(bool enter, GameObject other)
    {
        if (enter)
            OnEnterPlayer(other);
        else
            OnExitPlayer(other);



    }

    private void OnEnterPlayer(GameObject other)
    {
        if (_entetPlayerViewID != 0)
            return;
        var playerController = other.GetComponent<PlayerController>();
        if (playerController )
        {
            if(playerController.photonView.IsMine)
                SendDataToServer(playerController.ControllerNr, playerController.ViewID());
        }
    }

    private void OnExitPlayer(GameObject other)
    {
        var playerController = other.GetComponent<PlayerController>();
        if (playerController != null && playerController.photonView.ViewID == _entetPlayerViewID)
        {
            if(photonView.IsMine)
                SendDataToServer(0, 0);
            return;
        }
    }

    private void SendDataToServer(int controllerNr , int playerViewID)
    {
        _enterPlayerEvent.RaiseEventRemoveCached(new Hashtable { { PhotonKeyConfig.viewID, this.photonView.ViewID} });
        _enterPlayerEvent.RaiseEventToServer(new Hashtable {
                {PhotonKeyConfig.viewID,this.photonView.ViewID },
                { "cn",controllerNr},
                { PhotonKeyConfig.playerViewID, playerViewID} ,
                { "st", PhotonNetwork.ServerTimestamp}
        });
    }

    private IEnumerator UpdateTime()
    {
        while (true)
        {
            if(_entetPlayerViewID == 0)
            {
                _timeSlider.value = 0;
                break;
            }
            float remainTime = (_enterServerTime - PhotonNetwork.ServerTimestamp) * .001f + _getNeedTime;
            _timeSlider.value =  _getNeedTime - remainTime;
            if (remainTime < 0)
            {
                if(photonView.IsMine)
                    PhotonNetwork.Destroy(this.gameObject);
            }
            yield return null;
        }
    }

 

    #endregion

}
