using Photon.Pun;

using UnityEngine;
using UnityEngine.Events;

public class BulletProjectile : Projectile
{
    [SerializeField] private float _damageRange;
    [SerializeField] private LayerMask _attackLayer;
    private SphereCollider _collider;

    public UnityAction<int> onDamageCount;
    protected override void Awake()
    {
        base.Awake();
        _collider = GetComponent<SphereCollider>();
    }
    private void OnEnable()
    {
        _collider.enabled = true;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(this.transform.position, _endPoint, _speed * Time.deltaTime);
        var distance = Vector3.Distance(this.transform.position, _endPoint);
        if (distance == 0)
        {
            End();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Damageable damageable))
        {
            if (damageable.IsDead || _team == damageable.Team)
                return;
            End();
        }
    }


    protected override void End()
    {
        base.End();
        
        //var count = UtillGame.DamageInRange(this.transform.position, _damageRange, _damage, PlayerViewID, _attackLayer);
        //onDamageCount?.Invoke(count);
    }
}
