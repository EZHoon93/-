using System;
using System.Collections.Generic;

using UnityEngine;

// 추상 클래스
//[CreateAssetMenu(menuName = "RunTime/ ", fileName = "RunTime_")]

public abstract class RuntimeSet<T> : ScriptableObject
{
    public Action<T> onAddEvent;
    public Action<T> onRemoveEvent;
    public Action onChangeEvent;
    public List<T> Items = new List<T>();

    private void OnEnable()
    {
        Items.Clear();
    }
    public virtual void Add(T thing)
    {
        if (!Items.Contains(thing))
        {
            Items.Add(thing);
            onAddEvent?.Invoke(thing);
            onChangeEvent?.Invoke();
        }
    }

    public virtual void Remove(T thing)
    {
        if (Items.Contains(thing))
        {
            Items.Remove(thing);
            onRemoveEvent?.Invoke(thing);
            onChangeEvent?.Invoke();
        }
    }

    public int Count => Items.Count;
    
    
    
}