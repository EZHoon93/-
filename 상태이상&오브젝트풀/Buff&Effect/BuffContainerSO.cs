using System.Collections;
using System.Collections.Generic;
using System.Linq;

using EZPool;

using Sirenix.OdinInspector;

using UnityEngine;


[CreateAssetMenu(menuName = "Container/BuffContainerSO")]
public class BuffContainerSO : SerializedScriptableObject
{
    [SerializeField] private BuffPoolSO[] _buffPoolSOs;

    private Dictionary<int, BuffPoolSO> _buffDic = new Dictionary<int, BuffPoolSO>();


    private void OnEnable()
    {
        foreach(var buffPool in _buffPoolSOs)
        {
            _buffDic.Add(buffPool.Code, buffPool);
        }
    }
    private void OnDisable()
    {
        _buffDic.Clear();
    }


    public void Init()
    {

    }

    public BuffPoolSO GetBuffPoolSO(int code)
    {
        if(_buffDic.TryGetValue(code , out var result) == false)
        {
            result = _buffPoolSOs.Single(x => x.Code == code);
            if(result == null)
            {
                return null;
            }
            _buffDic.Add(code, result);
        }
        return result;
    }
}
