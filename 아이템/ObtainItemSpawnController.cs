using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public enum SpawnState
{
    Ready,
    Spanwed
}

public class ObtainItemSpawnController : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPoint
    {
        public SpawnState spawnState;
        public Vector3 spawnPos;
        public int index;
        public SpawnPoint(Vector3 pos, int i)
        {
            index = i;
            spawnPos = pos;
            spawnState = SpawnState.Ready;
        }
    }

    #region Varaibles
    [SerializeField] private GameObject _prefab;
    [SerializeField] private ItemConfigSO[] _obtainItems; //?????? ?????? ???????? ????,
    [SerializeField] private Define.Team _getTeam;  //?????????????????? 
    //[SerializeField] private Transform[] _initSpawnPoints;

    [SerializeField] int _maxCount;
    [SerializeField] float _timeBet;
    [SerializeField] float _initWaitTime;
    [SerializeField] float _updateTime;

    [Header("Listening")]
    [SerializeField] private VoidEventChannelSO _onStartGamingEventSO;
    [SerializeField] private IntEventChannelSO _createItemEvent;
    [SerializeField] private IntEventChannelSO _destroyItemEvent;
    [SerializeField] private SceneLoadEventChannelSO _sceneLoadEventChannelSO;

    int _lastCreateTime;
    int _currentSpawnCount;
    //[SerializeField] List<Transform> _canSpawnPointList; //???? ?????? ??????

    [SerializeField] private SpawnPoint[] _spawns;

    #endregion
    #region Properties
    #endregion

    #region Life Cycle
    private void OnEnable()
    {
        _onStartGamingEventSO.AddListener(CreateStart);
        _createItemEvent.AddListener(CallBackCreateItem);
        _destroyItemEvent.AddListener(CallBackDestroyItem);
        _sceneLoadEventChannelSO.OnLoadingRequested += OnLobbySceneLoad;
    }
    private void OnDisable()
    {
        _onStartGamingEventSO.RemoveListener(CreateStart);
        _createItemEvent.RemoveListener( CallBackCreateItem);
        _destroyItemEvent.RemoveListener(CallBackDestroyItem);
        _sceneLoadEventChannelSO.OnLoadingRequested -= OnLobbySceneLoad;

    }
    private void Start()
    {
        _currentSpawnCount = 0;
        CreateSpawnArrays();
    }
    #endregion

    #region public
    #endregion

    #region CallBack

    private void OnLobbySceneLoad(GameSceneSO gameSceneSO)
    {
        StopAllCoroutines();
    }

    private void CreateStart()
    {
        if (PhotonNetwork.IsMasterClient == false)
            return;
        
        StopAllCoroutines();
        StartCoroutine(UpdateCoroutine());
    }

    /// <summary>
    /// ???????? ?????? ???????? ????
    /// </summary>
    private void CallBackCreateItem(int spawnIndex)
    {
        if (spawnIndex > _spawns.Length)
            return;
        _spawns[spawnIndex].spawnState = SpawnState.Spanwed;
        _currentSpawnCount++;
    }
    private void CallBackDestroyItem(int spawnIndex)
    {
        if (spawnIndex > _spawns.Length)
            return;
        //???????????????? ?????? ?????????? ????????????
        if (_currentSpawnCount == _spawns.Length)
        {
            _lastCreateTime = PhotonNetwork.ServerTimestamp;
        }
        _spawns[spawnIndex].spawnState = SpawnState.Ready;
        _currentSpawnCount--;
    }
    #endregion

    #region private

    //??????, 
    private IEnumerator UpdateCoroutine()
    {
        yield return new WaitForSeconds(_initWaitTime);
        _lastCreateTime = PhotonNetwork.ServerTimestamp;
        var seconds = new WaitForSeconds(_updateTime);
        while (true)
        {
            if (CheckCanSpawn())
            {
                _lastCreateTime = PhotonNetwork.ServerTimestamp;
                Spawn();
            }
            yield return seconds;
        }
    }
    private void CreateSpawnArrays()
    {
        List<SpawnPoint> spawnList = new List<SpawnPoint>();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            var pos = this.transform.GetChild(i).position;
            var spawn = new SpawnPoint(pos, i);
            spawnList.Add(spawn);
        }

        _spawns = spawnList.ToArray();
    }
    //?????? ?? ?????? ????
    private bool CheckCanSpawn()
    {
        var remainTime  = _timeBet - (PhotonNetwork.ServerTimestamp - _lastCreateTime )* .001f  ;
        if(remainTime < 0 && _currentSpawnCount < _maxCount)
        {
            return true;
        }
        return false;
    }

    //????
    private void Spawn()
    {
        var randomSpawnPoint = GetRadnomSpawnPos();
        var obtainItemName = GetRandomItemData();
        PhotonNetwork.InstantiateRoomObject(_prefab.name, randomSpawnPoint.spawnPos, Quaternion.identity, 0 
            ,new object[] { randomSpawnPoint.index, _getTeam, obtainItemName});
    }

    /// <summary>
    /// ???????? ???????????? ?????? ???????? ????
    /// </summary>
    private string GetRandomItemData()
    {
        var ran = UnityEngine.Random.Range(0, _obtainItems.Length);

        return _obtainItems[ran].name;
    }
  
    /// <summary>
    /// ???? ?????? ?????? ?????????? 
    /// </summary>
    /// <returns></returns>
    private SpawnPoint GetRadnomSpawnPos()
    {
        var isSpawnable = _spawns.Any(x => x.spawnState == SpawnState.Ready);
        if (isSpawnable == false)
            return _spawns[0];
        


        var spawnPoint = _spawns.Where(x => x.spawnState == SpawnState.Ready).OrderBy(g => Guid.NewGuid()).First();
        return spawnPoint;
    }

  
    #endregion
}
