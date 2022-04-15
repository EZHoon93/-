using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WeaponType
{
    Projectile
}

[CreateAssetMenu(menuName = "Datas/WeaponInfo")]

public class WeaponInfoSO : DescriptionBaseSO
{
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private int _damage;
    [SerializeField] private float _range;
    [SerializeField] private float _distance;
    [SerializeField] private float _initSpeed;

    public WeaponType WeaponType => _weaponType;
    public int InitDamage => _damage;
    public float InitRange => _range;
    public float InitDistance => _distance;

    public float InitSpeed => _initSpeed;

}
