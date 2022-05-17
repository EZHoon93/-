using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;


public interface IPush<T>
{
    void Push(T pushItem);
}

[CreateAssetMenu (menuName = "Buff/Action")]
public abstract class BuffFactoryBase : ScriptableObject ,ICloneable 
{
    //public virtual List<Damageable> Damageables { get; set; } = new List<Damageable>();
    [SerializeField]
    private int _endTime;
    //private bool _isEnd;
    protected Damageable _target;

    public UnityAction onEndEvent;
    public Damageable Target => _target;
    public int EndTime => _endTime;


    public virtual bool IsPlay => PhotonNetwork.ServerTimestamp - _endTime < 0;

    
    public void SetupTarget(Damageable target)
    {
        EffectManager.Instance.onUpdateEffect += OnUpdate;
    }
    public void UpdateEndTime(int endTime)
    {
        _endTime = endTime;
    }

   
    private void OnUpdate()
    {
        if (IsPlay == false)
        {
            onEndEvent?.Invoke();
            onEndEvent = null;
            EffectManager.Instance.onUpdateEffect -= OnUpdate;
        }
    }
    

    protected abstract void StartAction();

    protected abstract void EndAction();

    

   
    public BuffFactoryBase Copy()
    {
        return (BuffFactoryBase)this.MemberwiseClone();
    }

    public abstract object Clone();

   
}
