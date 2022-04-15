using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class ObtainItemSpawnController : MonoBehaviour
{

    #region Varaibles
    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject[] _obtainItems; //생성할 아이템 랜덤으로 선택,
    [SerializeField] private Transform[] _initSpawnPoints;
    [SerializeField] int _maxCount;
    [SerializeField] float _timeBet;
    [SerializeField] float _initWaitTime;
    [SerializeField] float _updateTime;

    [Header("Listening")]
    [SerializeField] private VoidEventChannelSO _createStartEvent;
    //[SerializeField] private VoidEventChannelSO _masterChangeEvent;
    [SerializeField] private IntEventChannelSO _createItemEvent;
    [SerializeField] private IntEventChannelSO _destroyItemEvent;

    int _lastCreateTime;
    int _currentSpawnCount;
    List<Transform> _canSpawnPointList; //스폰 가능한 리스트
    #endregion
    #region Properties
    #endregion

    #region Life Cycle
    private void OnEnable()
    {
        _createStartEvent.onEventRaised += CreateStart;
        _createItemEvent.onEventRaised += CallBackCreateItem;
        _destroyItemEvent.onEventRaised += CallBackDestroyItem;
    }
    private void OnDisable()
    {
        _createStartEvent.onEventRaised -= CreateStart;
        _createItemEvent.onEventRaised -= CallBackCreateItem;
        _destroyItemEvent.onEventRaised -= CallBackDestroyItem;
    }
    private void Start()
    {
        _canSpawnPointList = _initSpawnPoints.ToList();
        _currentSpawnCount = 0;
    }
    #endregion

    #region public
    #endregion

    #region CallBack

    private void CreateStart()
    {
        if (PhotonNetwork.IsMasterClient == false)
            return;
        
        StopAllCoroutines();
        StartCoroutine(UpdateCoroutine());
    }

    /// <summary>
    /// 포톤으로 생성된 아이템이 호출
    /// </summary>
    private void CallBackCreateItem(int spawnIndex)
    {
        _canSpawnPointList.RemoveAt(spawnIndex);
        _currentSpawnCount++;
    }
    private void CallBackDestroyItem(int spawnIndex)
    {
        _canSpawnPointList.Add(_initSpawnPoints[spawnIndex]);
        _currentSpawnCount--;
    }
    //private void OnChangeMasterClient()
    //{

    //}
    #endregion

    #region private

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

    private bool CheckCanSpawn()
    {
        var remainTime  = _timeBet - (PhotonNetwork.ServerTimestamp - _lastCreateTime )* .001f  ;
        if(remainTime < 0 && _currentSpawnCount < _maxCount)
        {
            return true;
        }
        return false;
    }
    private void Spawn()
    {
        var selectPointIndex = GetSpawnPointIndex();
        var spawnPoint = GetSpawnPoint(selectPointIndex);
        var obtainItemName = GetRandomItemData();
        PhotonNetwork.InstantiateRoomObject(_prefab.name, spawnPoint, Quaternion.identity, 0 ,new object[] { selectPointIndex  , obtainItemName});
    }

    private string GetRandomItemData()
    {
        var ran = Random.Range(0, _obtainItems.Length);

        return _obtainItems[ran].name;
    }
  
    private int GetSpawnPointIndex()
    {
        return Random.Range(0, _canSpawnPointList.Count);
    }
    private Vector3 GetSpawnPoint(int index)
    {
        if(_initSpawnPoints.Length < index)
        {
            return Vector3.zero;
        }
        return _initSpawnPoints[index].position;
    }
    #endregion
}
