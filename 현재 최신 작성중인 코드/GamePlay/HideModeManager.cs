
using ExitGames.Client.Photon;

using Photon.Pun;

using UnityEngine;

public class HideModeManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject _seekerObject;
    [SerializeField] private GameObject _hiderObject;


    [Header("Listening")]
    [SerializeField] private EachPhotonViewEventChannelSO _eachPhotonViewEventChannelSO;
    [Header("BroadCasting")]
    [SerializeField] private PhotonEventChannelSO _spawnEventSO;

    #endregion
    #region Properties
    #endregion


    #region Life Cycle

    private void OnEnable()
    {
        _eachPhotonViewEventChannelSO.onEventRaised += OnPlayerSpawn;
    }

    private void OnDisable()
    {
        _eachPhotonViewEventChannelSO.onEventRaised -= OnPlayerSpawn;

    }
    #region Override, Interface
    #endregion
    #endregion
    #region public
    #endregion
    #region private

    private void OnPlayerSpawn(PhotonView photonView)
    {
        if (photonView.IsMine == false)
            return;

        var playerTeam = UtilPhoton.GetPlayerTeam(photonView);
        switch (playerTeam)
        {
            case Define.Team.Hide:
                PhotonNetwork.InstantiateRoomObject(_hiderObject.name, Vector3.down, Quaternion.identity, 0, new object[] { photonView.ViewID });
                break;
            case Define.Team.Seek:
                PhotonNetwork.InstantiateRoomObject(_seekerObject.name, Vector3.down, Quaternion.identity, 0, new object[] { photonView.ViewID });
                break;
        }
    }


    #endregion


}





