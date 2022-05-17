using System.Collections;
using System.Collections.Generic;

using EZPool;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.Events;

public enum ItemType
{
    Main = 0,
    Sub
}

[CreateAssetMenu(menuName = "Config/Item")]
public class ItemConfigSO : SerializedScriptableObject
{
    [BoxGroup("Enum")] [SerializeField] private InputDefine.UIType _uIType;
    [PropertyOrder(-1)] 
    [BoxGroup("Prefab")] [SerializeField] private Sprite _sprite;
    [BoxGroup("Prefab")] [SerializeField] private GameObject _modelPrefab;
    [BoxGroup("Prefab")] [SerializeField] private GameObject _zoomPrefab;
    [BoxGroup("Varaiables")]
    [TitleGroup("Varaiables/string")] [SerializeField] private string _useAnimationName;
    [TitleGroup("Varaiables/float")] [SerializeField] private float _initCoolTime;
    [TitleGroup("Varaiables/float")] [SerializeField] private float _initDistacne;
    [TitleGroup("Varaiables/float")] [SerializeField] private bool _isConsume;    //?????? ???????????? 

    [SerializeField] private ItemActionBase[] _itemActions;

    [ShowIf("isToggled")]
    [SerializeField] protected Dictionary<int, ItemSO> _itemDic = new Dictionary<int, ItemSO>();
    [ShowIf("isToggled")]
    [SerializeField] protected Stack<ItemSO> _itemSOStack = new Stack<ItemSO>();
    public bool isToggled;
    public InputDefine.UIType uIType => _uIType;
    public Sprite ItemSprite => _sprite;
    public float InitCoolTime => _initCoolTime;
    public string UseAnimName => _useAnimationName;
    public bool IsConsume => _isConsume;
    public GameObject ZoomPrefab => _zoomPrefab;
    public GameObject ModelPrefab => _modelPrefab;
    public ItemActionBase[] ItemActions => _itemActions;


    public float InitDistacne => _initDistacne;
    public int AnimKey => Animator.StringToHash(_useAnimationName);

    public virtual ItemSO GetItemSO(int viewID)
    {

        if (_itemDic.TryGetValue(viewID, out var result) == false)
        {
            result = PopItemSO();
            result.ViewID = viewID;
            result.ItemConfigSO = this;
            _itemDic.Add(viewID, result);
        }
        return result;
    }

    public void RemoveItemSO(int viewID)
    {
        if (_itemDic.ContainsKey(viewID))
        {
            _itemSOStack.Push( _itemDic[viewID]);
            _itemDic.Remove(viewID);
        }
    }

    protected ItemSO PopItemSO()
    {
        if(_itemSOStack.Count <= 0)
        {
            return CreateItemSO();
        }
        return _itemSOStack.Pop();
    }
    protected virtual ItemSO CreateItemSO()
    {
        var go = CreateInstance<ItemSO>();
        return go;
    }
    private void OnEnable()
    {
        Clear();
    }
    private void OnDisable()
    {
        Clear();
    }

    private void Clear()
    {
        foreach (var item in _itemDic)
            DestroyImmediate(item.Value);

        _itemDic.Clear();
        _itemSOStack.Clear();
    }
}
