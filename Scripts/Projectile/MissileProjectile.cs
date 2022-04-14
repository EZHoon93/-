
using EZPool;
using UnityEngine;
using UnityEngine.Events;

public class MissileProjectile : Projectile , ITriggerController
{
    [SerializeField] private float _damageRange;
    [SerializeField] private LayerMask _attackLayer;
    [SerializeField] private ParticlePoolSO _particleFactorySO;
    [SerializeField] private GameObject _effectPrefab;
    
    [Header("BroadCasting")]
    [SerializeField] private EffectEvnetChannelSO _effectEvnetChannelSO;


    public UnityEvent<int> onHitCount;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(this.transform.position, _endPoint, _speed * Time.deltaTime);
        var distance = Vector3.Distance(this.transform.position, _endPoint);
        if (distance == 0)
        {
            End();
        }
    }

    public void OnTriggerChangeDetected(bool enter, GameObject other)
    {
        //if (_isPlay == false || enter == false)
        //    return;
        //if (other.TryGetComponent(out Damageable damageable))
        //{
        //    if (damageable.IsDead || _team == damageable.Team)
        //        return;
        //    End();
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isPlay == false)
            return;
        if (other.TryGetComponent(out Damageable damageable))
        {
            if (damageable.IsDead || _team == damageable.Team)
                return;
            End();
        }
    }

    private void Explosion()
    {
        var count = UtillGame.DamageInRange(this.transform.position, _damageRange, _damage, _playerViewID, _attackLayer, _team);
        onHitCount?.Invoke(count);
        _effectEvnetChannelSO.RaiseEvent(_effectPrefab, this.transform.position);
    }
    protected override void End()
    {
        if (_isPlay == false)
            return;
        Explosion();
        base.End();
    }

    
}
