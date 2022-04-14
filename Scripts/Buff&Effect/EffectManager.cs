using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using EZPool;

public class EffectManager : MonoBehaviourPun
{
    [SerializeField] private int _initPoolSize;
    [SerializeField] GameObjectCodeSO[] _effectCodes;


    Dictionary<int, ParticlePoolSO> _effectDic = new Dictionary<int, ParticlePoolSO>();
    #region Varaibles
    [Header("Listening")]
    [SerializeField] private EffectEvnetChannelSO _effectEvnetChannelSO;
    #endregion
    #region Properties
    #endregion

    #region Life Cycle
    private void OnEnable()
    {
        _effectEvnetChannelSO.onEventRaised += OnEffect;
    }
    private void OnDisable()
    {
        _effectEvnetChannelSO.onEventRaised -= OnEffect;
    }


    #endregion
    #region Override, Interface
    #endregion

    #region public
    #endregion

    #region CallBack

    private void OnEffect(GameObject particlePrefab , Vector3 position, bool isRPC = false)
    {
        var code = particlePrefab.GetInstanceID();
        ParticleSystem effectParticle = null;

        if (_effectDic.TryGetValue(code, out var particlePoolSO) == false)
        {
            particlePoolSO = CreatePoolSO(particlePrefab);
            if (particlePoolSO == null)
            {
                Debug.LogError("Paractile Pool SO  Null");
                return;
            }
            _effectDic.Add(code, particlePoolSO);
        }
        effectParticle = particlePoolSO.Pop();
        effectParticle.transform.position = position;
        StartCoroutine( ProcessParticle(particlePoolSO, effectParticle));
    }

 

    #endregion

    #region private
    private ParticlePoolSO CreatePoolSO(GameObject particlePrefab)
    {
        var particleSystem =  particlePrefab.GetComponent<ParticleSystem>();
        if (particleSystem == null)
            return null;
        
        var poolSO = ScriptableObject.CreateInstance<ParticlePoolSO>();
        var factorySO = ScriptableObject.CreateInstance<ParticleFactorySO>();
#if UNITY_EDITOR
        poolSO.name = $"Pool {particleSystem.name}";
#endif

        factorySO.SetupPrefab(particleSystem);
        poolSO.Factory = factorySO;
        poolSO.Prewarm(_initPoolSize);
        poolSO.SetParent(this.transform);   //계속 재사용을위해..

        return poolSO;
    }

    private IEnumerator ProcessParticle(ParticlePoolSO poolSO , ParticleSystem particleSystem)
    {
        particleSystem.Play();
        var time = particleSystem.main.duration;
        yield return new WaitForSeconds(time);
        poolSO.Push(particleSystem); 
    }
    #endregion

}
