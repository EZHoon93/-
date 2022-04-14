using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ( menuName = "GameObjectCode")]
public class GameObjectCodeSO : DescriptionBaseSO
{
    [SerializeField] private int _code;
    [SerializeField] private GameObject _prefab;

    public int Code => _code;
    public GameObject Prefab => _prefab;
}
