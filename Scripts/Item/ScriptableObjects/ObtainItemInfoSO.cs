using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName =  "Data/ObtainItem")]
public class ObtainItemInfoSO : DescriptionBaseSO
{
    [SerializeField] private float _getNeedTime;
    [SerializeField] private GameObject _prefab;

    public float GetNeedTime => _getNeedTime;
    public GameObject Prefab => _prefab;
}
