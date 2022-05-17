using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using EZPool;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using Photon.Realtime;
using UnityEngine.Events;

//public class ApplyBuffInfo
//{
//    Dictionary<int, BuffFactoryBase> _buffDic = new Dictionary<int, BuffFactoryBase>();
    
//    public bool HasBuff(int buffCode)
//    {
//        return _buffDic.ContainsKey(buffCode);
//    }

//    public BuffFactoryBase GetBuff(int buffCode)
//    {
//        if (HasBuff(buffCode))
//        {
//            return _buffDic[buffCode];
//        }

//        return null;
//    }

//    public int GetBuffEndTime(int buffCode)
//    {
//        if (HasBuff(buffCode))
//           return GetBuff(buffCode).EndTime;

//        return 0;
//    }

//    public void Add(int buffCode, BuffFactoryBase buff)
//    {
//        if (_buffDic.ContainsKey(buffCode) == false)
//            _buffDic.Add(buffCode, buff);
//    }

//    public void Remove(int buffCode)
//    {
//        _buffDic.Remove(buffCode);
//    }

//    public Dictionary<int,int> ConvertToSendServerData()
//    {
//        return _buffDic.Select(x => x).ToDictionary(x=>x.Key, x => x.Value.EndTime);
//    }

//    public void Clear()
//    {
//        _buffDic.Clear();
//    }

//}
public class EffectManager : GenricSingleton<EffectManager>
{
    [AssetList(Path = "ScriptableObjects/Pool/Buff")]
    [SerializeField] private BuffPoolSO[] _buffPoolSOs;
    //[SerializeField] private BuffController _buffControllerPrefab;
    //[AssetList(Path = "ScriptableObjects/Pool/Effect")]
    //[SerializeField] private ParticlePoolSO[] _particlePoolSOs;
    //[SerializeField] private DamageableRunTimeSet _damageableRunTimeSet;
    #region Varaibles
    [Title("Listening")]
    //[FoldoutGroup("Event")] [SerializeField] private TransformEventChannelSO _changeCameraTagetEventSO;
    //[SerializeField] private PhotonEventChannelSO _buffEventChannelSO;
    //[SerializeField] private Dictionary<int, ApplyBuffInfo> _damageableBuffDic = new Dictionary<int, ApplyBuffInfo>();
    //[SerializeField] private PhotonViewEventChannelSO _onSubPlayerSpawnEventSO;
    //[SerializeField] private PhotonViewEventChannelSO _onSubPlayerDisableEventSO;

    //[SerializeField] private IntEventChannelSO _onCameraChangeEvnetSO;
    //[SerializeField]
    //Dictionary<int, BuffController> _buffDic = new Dictionary<int, BuffController>();

    //List<ParticlePoolSO>

    public UnityAction onUpdateEffect;
    public UnityAction<ParticlePoolSO> ond;
    WaitForSeconds _updateTiemSeconds = new WaitForSeconds(.2f);
    WaitForSeconds _particleWaitSeconds = new WaitForSeconds(3.0f);

    #endregion
    #region Properties
    #endregion

    #region LifyCycle

    private void OnEnable()
    {
        //_onSubPlayerSpawnEventSO.AddListener(OnSpawnSubPlayer);
        //_onSubPlayerDisableEventSO.AddListener(OnDsiableSubPlayer);

        StartCoroutine(UpdateCorutine());
    }

    private void OnDisable()
    {
        //_onSubPlayerSpawnEventSO.RemoveListener(OnSpawnSubPlayer);
        //_onSubPlayerDisableEventSO.RemoveListener(OnDsiableSubPlayer);


    }

    private void Clear()
    {
        //foreach(var damageable in _damageableBuffDic.ToArray())
        //{
        //    damageable.Value.Clear();
        //}
    }
    private IEnumerator UpdateCorutine()
    {
        while (true)
        {
            onUpdateEffect?.Invoke();
            yield return _updateTiemSeconds;
        }
    }
    //public IEnumerator Timer(ParticlePoolSO poolSO, ParticleSystem particleSystem)
    //{
    //    particleSystem.Play();
    //    while (particleSystem.isPlaying)
    //    {
    //        yield return null;
    //    }
    //    poolSO.Push(particleSystem);
    //}

    #endregion
    #region OnEvent
    ////데이터를위한 서브오브젝 생성시 
    //private void OnSpawnSubPlayer(PhotonView photonView)
    //{
    //    var data = photonView.InstantiationData;
    //    if (data == null || _buffDic.ContainsKey(photonView.ViewID))
    //        return;
    //    var buffController = photonView.gameObject.GetOrAddComponent<BuffController>();
    //    _buffDic.Add(photonView.ViewID, buffController);
    //}

    //private void OnDsiableSubPlayer(PhotonView photonView)
    //{
    //    if (_buffDic.ContainsKey(photonView.ViewID) == false)
    //    {
    //        return;
    //    }
    //    if (photonView.TryGetComponent(out Damageable damageable))
    //    {
    //        Destroy(damageable);
    //    }
    //    _buffDic.Remove(photonView.ViewID);
    //}

    //private BuffController PopBuffController()
    //{
    //    return null;
    //}



    //private void 

    //버프시작
    //private void OnBuffByServer(object data)
    //{
    //    var datas = (object[])data;
    //    var viewID = (int)datas[0];
    //    var buffCode = (int)datas[1];
    //    var endTime = (int)datas[2];
    //    ApplyBuff(viewID, buffCode, endTime);
    //}

    //private void OnIntergrestGroupChange(int[] eanbleGroups)
    //{
    //    foreach(var damageable in _damageableRunTimeSet.Items)
    //    {
    //        if (eanbleGroups.Contains(damageable.photonView.Group))
    //        {
    //            //return;//버프생성 & 체크
    //        }
    //    }
    //}



    #endregion
    //public void LocalSendBuffToServer(int viewID, int buffCode, float durationTime)
    //{
    //    int endTime = PhotonNetwork.ServerTimestamp + (int)(1000 * durationTime);
    //    //ApplyBuff(viewID, buffCode, endTime);
    //    //_buffEventChannelSO.RaiseEventToServer(new object[] { viewID, buffCode, endTime }, ReceiverGroup.Others);
    //    if (_buffDic.ContainsKey(viewID) == false)
    //    {
    //        return;
    //    }

    //    var buffConttroller = _buffDic[viewID];
    //    buffConttroller.Setup(buffCode, endTime);
    //}



    //public BuffPoolSO GetBuffPoolSO(int buffCode)
    //{
    //    var go = _buffPoolSOs.SingleOrDefault(x => x.Code == buffCode);
    //    return go;
    //}

    //private void ApplyBuff(int viewID, int buffCode , int endTime)
    //{
    //    var target = _damageableRunTimeSet.GetItem(viewID);
    //    if(target == null)
    //    {
    //        return;
    //    }

    //    if (_damageableBuffDic.TryGetValue(viewID, out var applyBuffInfo) == false)
    //    {
    //        applyBuffInfo = new ApplyBuffInfo();
    //        _damageableBuffDic.Add(viewID, applyBuffInfo);
    //    }
    //    //최초 생성
    //    if (applyBuffInfo.HasBuff(buffCode) == false)
    //    {

    //        var buffPoolSO = GetBuffPoolSO(buffCode);
    //        var particle = buffPoolSO.Pop();
    //        var buff = buffPoolSO.GetBuffAction();
    //        buff.UpdateEndTime(endTime);
    //        particle.transform.SetParent(target.transform);
    //        particle.transform.localPosition = buffPoolSO.AddPos;
    //        applyBuffInfo.Add(buffCode, buff);
    //        buff.onEndEvent += () => {
    //            applyBuffInfo.Remove(buffCode);
    //            buffPoolSO.PushAction(buff);
    //        };
    //        StartCoroutine(Timer(buffPoolSO, particle, buff));
    //    }
    //    //시간 업데이트
    //    else
    //    {
    //        var buff = applyBuffInfo.GetBuff(buffCode);
    //        buff.UpdateEndTime(endTime);
    //    }
    //}

    //public IEnumerator ParticlePushProcess(ParticlePoolSO poolSO, ParticleSystem particleSystem)
    //{
    //    while (particleSystem.isPlaying)
    //    {
    //        yield return null;
    //    }
    //}





}
