using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Main = 0,
    Sub
}

[CreateAssetMenu (menuName = "Datas/Item")]
public class ItemSO : DescriptionBaseSO
{
    [SerializeField] private ItemType _itemType;
    [SerializeField] private InputDefine.UIType _uIType;
    [SerializeField] private float _getNeedTime;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private GameObject _modelPrefab;
    [SerializeField] private GameObject _zoomPrefab;
    [SerializeField] private string _useAnimationName;
    [SerializeField] private float _initCoolTime;
    [SerializeField] private bool _isConsume;    //소모품 일회성아이템 


    public ItemType ItemTypes=> _itemType;
    public InputDefine.UIType uIType => _uIType;
    public Sprite ItemSprite => _sprite;
    public float InitCoolTime => _initCoolTime;
    public string UseAnimName => _useAnimationName;
    public float GetNeedTime => _getNeedTime;
    public bool IsConsume => _isConsume;
    public GameObject ZoomPrefab => _zoomPrefab;
    public GameObject ModePrefab => _modelPrefab;
}
