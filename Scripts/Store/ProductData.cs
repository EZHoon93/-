using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProductData_", menuName = "EZ/CreateProductData")]
public class ProductData : ScriptableObject
{
    [SerializeField] GameObject _prefab;
    [SerializeField] string _key;

    public GameObject Prefab => _prefab;
    public string Key => _key;

    
}
