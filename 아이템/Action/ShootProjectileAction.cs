
using System;
using System.Collections;

using DG.Tweening;

using EZPool;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Action/ShootProjectileAction", fileName = "ItemAction ShootProjectileAction")]
public class ShootProjectileAction : ItemActionBase
{
    [SerializeField] private TransformPoolSO _waringPoolSO;
    [SerializeField] private ParticlePoolSO _expolsionPoolSO;
    [SerializeField] private ProjectilePoolSO _projectilePoolSO;
    [SerializeField] private Ease Ease1;
    [SerializeField] private Ease Ease2;

    [SerializeField] private float time;


    protected static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

    public override void UseOnServer(MyMonoBehaviourPun runner, ItemSO itemSO)
    {
        WeaponSO weaponSO = itemSO as WeaponSO;
        Debug.Log("Log Delay UseOnServer" + weaponSO);

        if (weaponSO == null)
        {
            Debug.LogError("Error Not WeapSO Type");
            //return;
        }

        runner.StartCoroutine(ProcessProjectile(weaponSO));
    }

    private IEnumerator ProcessProjectile(WeaponSO weaponSO)
    {
        var go = _projectilePoolSO.Pop();
        var targetPoint = weaponSO.TargetPoint;
        var startPoint = weaponSO.StartPoint;
        var distance = Vector3.Distance(startPoint, targetPoint);
        var arriveTime = distance * .3f;

        var warningUI = _waringPoolSO.Pop();
        warningUI.transform.position = targetPoint;
        warningUI.transform.localScale = Vector3.one * weaponSO.Range * 2;
        go.transform.position = startPoint;
        go.transform.DOMoveX(targetPoint.x, arriveTime).SetEase(Ease.Linear);
        go.transform.DOMoveZ(targetPoint.z, arriveTime).SetEase(Ease.Linear);
        go.transform.DOMoveY(0.3f + distance * .5f , arriveTime *.5f).SetEase(Ease1);
        yield return new WaitForSeconds(arriveTime * .5f);
        go.transform.DOMoveY(targetPoint.y, arriveTime * .5f).SetEase(Ease2);
        yield return new WaitForSeconds(arriveTime * .5f);
        ProcessOnDamage(weaponSO);
        _expolsionPoolSO.Pop(weaponSO.TargetPoint);
        _waringPoolSO.Push(warningUI);
        _projectilePoolSO.Push(go);
    }

    //private IEnumerator DelayAttack(WeaponSO weaponSO)
    //{
    //    Debug.Log("Test ..");

    //    while (true)
    //    {
    //        Debug.Log("Test ..");
    //        yield return null;
    //    }
    //    var targetPoint = weaponSO.TargetPoint;
    //    var damage = weaponSO.Damage;
    //    var range = weaponSO.Range;
    //    yield return new WaitForSeconds(.3f);
    //    var waringUI = _waringPoolSO.Pop();
    //    waringUI.transform.position = targetPoint;
    //    waringUI.transform.localScale = Vector3.one * range*2 ;
    //    yield return new WaitForSeconds(.3f);
    //    _expolsionPoolSO.Pop(targetPoint,range);
    //    ProcessOnDamage(weaponSO);
    //    _waringPoolSO.Push(waringUI);

    //}

    private void ProcessOnDamage(WeaponSO weaponSO)
    {
        var targetPoint = weaponSO.TargetPoint;
        var damageLayerMask = weaponSO.DamageLayerMssk;
        var range = weaponSO.Range;
        var ourTeam = weaponSO.PlayerController.Team;
        var damage = weaponSO.Damage;
        var usePlayerViewID = weaponSO.PlayerController.photonView.ViewID;
        Collider[] colliders = new Collider[10];

        var hitCount = Physics.OverlapSphereNonAlloc(targetPoint, range, colliders, damageLayerMask);
        if (hitCount > 0)
        {
            for (int i = 0; i < hitCount; i++)
            {
                var damageable = colliders[i].gameObject.GetComponent<Damageable>();
                if (damageable != null)
                {
                    if (damageable.Team == ourTeam)
                        continue;
                    damageable.ReceiveAnAttack(usePlayerViewID, damage, colliders[i].transform.position);
                }
            }
        }
    }
    
    private void OnShoot(WeaponSO weaponSO)
    {
        //var targetPoint = weaponSO.TargetPoint;
        //var damageLayerMask = weaponSO.DamageLayerMssk;
        //var range = weaponSO.Range;
        //var ourTeam = weaponSO.PlayerController.Team;
        //var damage = weaponSO.Damage;
        //var usePlayerViewID = weaponSO.PlayerController.photonView.ViewID;
        //var startPoint = weaponSO.StartPoint;

        //var projectile = _projectilePoolSO.Pop();
        //projectile.transform.position = startPoint;
        //projectile.Setup(ourTeam, usePlayerViewID, damage, targetPoint);\

        //var waringUI = _projectilePoolSO.Pop();
        //waringUI.transform.position = weaponSO.TargetPoint;
        
    }

  
}
