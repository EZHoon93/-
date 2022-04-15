using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkinType
{
    Character,
    Weapon,
    Accesoriess
}

[CreateAssetMenu(menuName = "Container/SkinContainerSO")]
public class SkinContainerSO : ScriptableObject
{
    [SerializeField] GameObject[] _characterSkins;
    [SerializeField] GameObject[] _weaponSkins;
    [SerializeField] GameObject[]  _accessoriessSkins;


    public GameObject[] CharacterSkins => _characterSkins;
    public GameObject[] WeaponSkins => _weaponSkins;
    public GameObject[] AccessoreissSkins => _accessoriessSkins;



    public GameObject CreateSkinObject(SkinType skinType,  string key)
    {
        GameObject skinPrefab = null;
        switch (skinType)
        {
            case SkinType.Accesoriess:
                skinPrefab = null;
                break;
            case SkinType.Character:
                skinPrefab = CreateCharacterSkinObject(key);
                break;
            case SkinType.Weapon:
                skinPrefab = CreateWeaponSkinObject(key);
                break;
        }
        if(skinPrefab == null)
        {
            return null;
        }
        var go = Instantiate(skinPrefab);

        return go;
    }

    GameObject CreateCharacterSkinObject(string key)
    {

        return _characterSkins[0].gameObject;
    }
    GameObject CreateWeaponSkinObject(string key)
    {
        return _weaponSkins[0].gameObject;
    }
    GameObject CreateAccessoriessSkinObject(string key)
    {
        return null;
    }
}
