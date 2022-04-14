using Photon.Pun;

using UnityEngine;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using EZPool;

public class Weapon_Seeker : Weapon , IPunInstantiateMagicCallback
{
    [SerializeField] private int _initialPoolSize = 5;
    [SerializeField] int _noHitDamage; 
    TransformPoolSO _skinPoolSO;
    TransformFactorySO _skinFactorySO;

    [Header("T E S T")]
    [SerializeField] GameObject _skinTest;

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        _skinPoolSO = ScriptableObject.CreateInstance<TransformPoolSO>();
        _skinFactorySO = ScriptableObject.CreateInstance<TransformFactorySO>();
        _skinPoolSO.name = $"Pool {_skinTest.name}";
        _skinFactorySO.SetupPrefab(_skinTest.transform);
        _skinPoolSO.Factory = _skinFactorySO;
        _skinPoolSO.Prewarm(_initialPoolSize);

        //var HT = (Hashtable)info.photonView.InstantiationData[1];
        //_skinPrefab = Managers.ProductSetting.GetSkinPrefab(Define.ProductType.Weapon, (int)HT["we"]);
        //_skinPrefab = Managers.ProductSetting.GetSkinPrefab(Define.ProductType.Weapon, 0);
    }
    protected override Projectile Pop()
    {
        var go = _projectilePoolSO.Pop() as MissileProjectile;
        var skinObject = _skinPoolSO.Pop();

        skinObject.transform.ResetTransform(go.PorjectileModel.transform);
        go.onPushEvent += () =>
        {
            _skinPoolSO.Push(skinObject);
        };
        return go;
    }

}
