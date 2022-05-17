using UnityEngine;
using Photon.Pun;
using System.Collections;
using UnityEngine.UI;
using EZPool;

public class ObtainItemController : MyMonoBehaviourPun, IPunInstantiateMagicCallback , ITriggerController
{
    #region Varaibles
    [Header("Varaiables")]
    [SerializeField] private GameObject _itemPrefab;
    [Header("Listening")]
    [SerializeField] private VoidEventChannelSO _onPlayerDieEvent;
    [Header("BroadCasting")]
    [SerializeField] private IntEventChannelSO _createItemEvent;
    [SerializeField] private IntEventChannelSO _destroyItemEvent;
    [Header("Component")]
    [SerializeField] private Transform _modelPivot;
    [SerializeField] private ParticlePoolSO _getPoolSO;
    [SerializeField] private ParticleSystem _onStartEffect;
    [SerializeField] private Slider _timeSlider;
    [SerializeField] private float _getNeedTime;

    private int _enterServerTime;
    private int _entetPlayerViewID; //???? ???????? ???????? ????????
    PlayerController _gettingPlayer;
    public int spawnIndex;
    [SerializeField] private Define.Team _getCanTeam;    //?????????? ??
    //private bool isPlay;
    #endregion
    #region Properties
    #endregion

    #region Life Cycle
    private void OnEnable()
    {
    }
    private void OnDisable()
    {
        var spawnIndex = (int)this.photonView.InstantiationData[0];
        _destroyItemEvent.RaiseEvent(spawnIndex);
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
         spawnIndex = (int)info.photonView.InstantiationData[0];
        _getCanTeam = (Define.Team)info.photonView.InstantiationData[1];
        _timeSlider.value = 0; 
        _timeSlider.maxValue = _getNeedTime;
        _createItemEvent.RaiseEvent(spawnIndex);
        _onStartEffect.Play();
    }
    #endregion

    #region public
    #endregion

    #region CallBack

 
    private void OnPlayerControllerDie()
    {
        StopAllCoroutines();
        _entetPlayerViewID = 0;
        _enterServerTime = 0;
    }

    [PunRPC]
    public void OnCallBack(int enterTime, int playerViewID)
    {
        _enterServerTime = enterTime;
        _entetPlayerViewID = playerViewID;
        StopAllCoroutines();
        if (_enterServerTime == 0)
        {
            _timeSlider.value = 0;
        }
        else
        {
            StartCoroutine(UpdateTime());
        }
    }
    [PunRPC]
    public void OnGetEffectByServer()
    {
        _getPoolSO.Pop(this.transform.position);
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
        _gettingPlayer = other.GetComponent<PlayerController>();
        if (_gettingPlayer != null && _gettingPlayer.photonView.IsMine  && _gettingPlayer.Team == _getCanTeam)
        {
            photonView.RPC("OnCallBack", RpcTarget.All,  PhotonNetwork.ServerTimestamp , _gettingPlayer.photonView.ViewID);
        }
    }

    private void OnExitPlayer(GameObject other)
    {
        var playerController = other.GetComponent<PlayerController>();
        if (playerController != null && playerController.photonView.ViewID == _entetPlayerViewID)
        {
            if (photonView.IsMine)
            {
                photonView.RPC("OnCallBack", RpcTarget.All, 0, 0);
            }
            return;
        }
    }

    private IEnumerator UpdateTime()
    {
        float remainTime =_getNeedTime;
        while (remainTime > 0)
        {
            remainTime = (_enterServerTime - PhotonNetwork.ServerTimestamp) * .001f + _getNeedTime;
            _timeSlider.value =  _getNeedTime - remainTime;
            yield return null;
        }
        if (photonView.IsMine)
        {
            var obtainItemKey = (string)photonView.InstantiationData[2];
            this.photonView.RPC("OnGetEffectByServer", RpcTarget.All);
            PhotonNetwork.InstantiateRoomObject(_itemPrefab.name, Vector3.down, Quaternion.identity, 0, new object[] { _gettingPlayer.ControllerNr, _entetPlayerViewID, obtainItemKey, ItemType.Sub });
            PhotonNetwork.Destroy(this.gameObject);
        }
    }



    #endregion

}
