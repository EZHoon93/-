using UnityEngine;

[CreateAssetMenu(fileName = "Weapons", menuName = "EntityConfig/Player's Health")]
public class WeaponSO : ItemSO
{
    private int _damage;
    private float _range;
    public LayerMask DamageLayerMssk 
    { 
        get; 
        set; 
    }

  
    public int Damage 
    {
        get => _damage;
        set
        {
            _damage = value;
        }
    }

    public float Range
    {
        get => _range;
        set
        {
            _range= value;
        }
    }



    public override void Reset()
    {
        var weaponConfigSO = ItemConfigSO as WeaponConfigSO;
        if (weaponConfigSO)
        {
            Damage = weaponConfigSO.InitDamage;
            Range = weaponConfigSO.InitRange;
        }
        base.Reset();
    }
}
