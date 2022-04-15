using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "HealthConfigSO_", menuName = "EntityConfig/HealthConfigSO")]
public class HealthConfigSO : ScriptableObject
{
    [SerializeField] private int _initHealth;

    public int InitHealth => _initHealth;
}
