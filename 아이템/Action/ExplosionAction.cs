
using System.Collections;

using EZPool;
using UnityEngine;
using UnityEngine.Events;


//[System.Serializable]
//public class DamageableEvent : UnityEvent<Damageable,int>
//{

//}

[CreateAssetMenu(menuName = "Item/Action/Seeker Attack" , fileName = "ItemAction SeekerAttack")]
public class ExplosionAction : ItemActionBase
{
    [SerializeField] private ParticlePoolSO _particlePoolSO;
    [SerializeField] private UnityEvent<ItemSO, int> onHitEvent;  //UsePlayer , hitCount
    [SerializeField] private UnityEvent<Damageable> onApplyEvent;


    public override void UseOnServer(MyMonoBehaviourPun runner, ItemSO itemSO)
    {
        WeaponSO weaponSO = itemSO as WeaponSO;
        if (weaponSO == null)
        {
            Debug.LogError("Error Not WeapSO Type");
        }

        runner.StartCoroutine(DelayAttack(weaponSO));

    }

    private IEnumerator DelayAttack(WeaponSO weaponSO)
    {
        yield return new WaitForSeconds(.5f);
        OnDamage(weaponSO);
    }
    private void OnDamage(WeaponSO weaponSO)
    {
        var targetPoint = weaponSO.TargetPoint;
        var damageLayerMask = weaponSO.DamageLayerMssk;
        var range = weaponSO.Range;
        var ourTeam = weaponSO.PlayerController.Team;
        var damage = weaponSO.Damage;
        var usePlayerViewID = weaponSO.PlayerController.photonView.ViewID;

        Collider[] colliders = new Collider[10];

        var hitCount = Physics.OverlapSphereNonAlloc(targetPoint, range, colliders, damageLayerMask);
        int damageCouunt = 0;
        if (hitCount > 0)
        {
            for (int i = 0; i < hitCount; i++)
            {
                var damageable = colliders[i].gameObject.GetComponent<Damageable>();
                if (damageable != null)
                {
                    if (damageable.Team == ourTeam)
                        continue;
                    damageCouunt++;
                    damageable.ReceiveAnAttack(usePlayerViewID, damage, colliders[i].transform.position);
                    onApplyEvent?.Invoke(damageable);
                }
            }
        }
        _particlePoolSO.Pop(targetPoint , range);
        onHitEvent?.Invoke(weaponSO, damageCouunt);
    }
}
