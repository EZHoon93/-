using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ProjectileConfigSO", menuName = "Config/ProjectileConfigSO")]

public class ProjectileConfigSO : DescriptionBaseSO
{
    [SerializeField] private int _initDamage;
    [SerializeField] private float _initSpeed;

    public int InitDamage => _initDamage;
    public float InitSpeed => _initSpeed;

}
