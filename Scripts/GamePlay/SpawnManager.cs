using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;

public class SpawnManager  : MonoBehaviour
{

    #region Fields
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _AIPlayerPrefab;
    [Header("Varaibles")]
    [SerializeField] private PhotonGamingPlayersInfoSO _photonGamingPlayersInfoSO;
    [Header("Listening")]
    [SerializeField] private IntEventChannelSO _onTeamPlayersSpawnEvent;
    GameObject _spawnObject;
    #endregion
    #region Properties
    #endregion

    #region Life Cycle
 
    private void OnEnable()
    {
        _onTeamPlayersSpawnEvent.onEventRaised += OnPlayerSpawn;
    }
    private void OnDisable()
    {
        _onTeamPlayersSpawnEvent.onEventRaised -= OnPlayerSpawn;
    }
    #endregion
    #region Override, Interface
    #endregion
    #region public



    #endregion


    #region private


    private void OnPlayerSpawn(int teamCode)
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            return;
        }

        if (_spawnObject ==  null)
            _spawnObject = GameObject.FindGameObjectWithTag("Respawn");

        var spawn =  _spawnObject.transform.GetChild(teamCode);
        if(spawn == null)
        {
            return;
        }
        var team = (Define.Team)teamCode;
        //List<Transform> spawnPointList = this.GetComponentsChildrenRemoveMy<Transform>().ToList();
        List<Transform> spawnPointList = spawn.GetComponentsChildrenRemoveMy<Transform>().ToList();
        Vector3 spawnPos = Vector3.zero;
        foreach (var playerInfo in _photonGamingPlayersInfoSO.Data)
        {
            int playerControllerNr = (int)playerInfo.Key;
            var dataHT = (Hashtable)playerInfo.Value;
            //print(playerControllerNr + "/" + (Define.Team)dataHT[PunHashKeyConfig.team]);

            if ((Define.Team)dataHT[PunHashKeyConfig.team] != team)
                continue;

            var ranTransform = UtillGame.GetRandomItem(ref spawnPointList);
            spawnPos = ranTransform.position;
            GameObject player = null;
            if (playerControllerNr > 0)
                 player = PhotonNetwork.InstantiateRoomObject(_playerPrefab.name, spawnPos, Quaternion.identity, 0,
                new object[] { playerControllerNr, dataHT, });
            else
                player = PhotonNetwork.InstantiateRoomObject(_AIPlayerPrefab.name, spawnPos, Quaternion.identity, 0,
                new object[] { playerControllerNr, dataHT, });

        }
    }


    #endregion


}
