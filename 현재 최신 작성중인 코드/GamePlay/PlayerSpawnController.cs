using System.Collections;
using System.Collections.Generic;


using UnityEngine;

public class PlayerSpawnController : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] Transform _spawnPoints;

    [Header("Listening")]
    [SerializeField] private VoidEventChannelSO _onPlayerSpawnEvent;
    #endregion
    #region Properties
    #endregion

    #region Life Cycle
    private void OnEnable()
    {
        _onPlayerSpawnEvent.onEventRaised += OnPlayerSpawn;
    }
    private void OnDisable()
    {
        _onPlayerSpawnEvent.onEventRaised -= OnPlayerSpawn;
    }

    #endregion
    #region Override, Interface
    #endregion
    #region public
    #endregion


    #region private
    private void OnPlayerSpawn()
    {
        
    }
    #endregion


}
