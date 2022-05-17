using System.Collections;
using System.Collections.Generic;

using Sirenix.OdinInspector;

using UnityEngine;


public enum WeaponType
{
    Projectile,
    Melee
}


[CreateAssetMenu(menuName = "Config/WeaponConfig")]
public class WeaponConfigSO : ItemConfigSO
{

    [BoxGroup("Enum")] [SerializeField] private LayerMask _damageLayerMask;
    [BoxGroup("Enum")] [SerializeField] private WeaponType _weaponType;
    [TitleGroup("Varaiables/int")] [SerializeField] private int _damage;
    [TitleGroup("Varaiables/float")] [SerializeField] private float _range;

    public WeaponType WeaponType => _weaponType;
    public int InitDamage => _damage;
    public float InitRange => _range;

    

    public override ItemSO GetItemSO(int viewID)
    {
        var itemSO = base.GetItemSO(viewID);
        var weaponSO = itemSO as WeaponSO;
        if(weaponSO)
        {
            weaponSO.DamageLayerMssk = _damageLayerMask;
        }
        return itemSO;
    }

    protected override ItemSO CreateItemSO()
    {
        var go = CreateInstance<WeaponSO>();
        return go;
    }

}
