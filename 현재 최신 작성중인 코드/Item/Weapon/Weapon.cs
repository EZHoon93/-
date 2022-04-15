using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using EZPool;

[RequireComponent(typeof(InputControllerObject))]

public class Weapon : MonoBehaviourPun , IInputObject 
{
    [SerializeField] private WeaponInfoSO _weaponInfoSO;
    [SerializeField] protected Transform _fireTransform;
    [SerializeField] protected Projectile _projectilePrefab;
    [SerializeField] protected ProjectilePoolSO _projectilePoolSO;
    [SerializeField] LayerMask _layerMask;

    int _damage;
    float _maxDistance;
    float _maxRange;
    float _speed;
    public event Action<float> onChangeDistance;
    public event Action<float> onChangeRange;
    public event Action onDestroyEvent;
    public float MaxDistance
    {
        get => _maxDistance;
        set
        {
            _maxDistance = value;
            onChangeDistance?.Invoke(value);
        }
    }
    
    public float MaxRange
    {
        get => _maxRange;
        set
        {
            _maxRange = value;
            onChangeRange?.Invoke(value);

        }
    }
    private void OnEnable()
    {
        MaxDistance = _weaponInfoSO.InitDistance;
        MaxRange = _weaponInfoSO.InitRange;
        _damage = _weaponInfoSO.InitDamage;
        _speed = _weaponInfoSO.InitSpeed;
    }
    private void OnDisable()
    {
        onChangeRange = null;
        onChangeDistance = null;
        onDestroyEvent?.Invoke();
    }
    [PunRPC]
    protected void UseOnServer(int playerViewID, Vector3 endPoint)
    {
        StartCoroutine(ProcessAttackOnAllClinets(playerViewID, endPoint));
    }

    public void Use(int playerViewID, Vector3 inputVector)
    {
        var endPoint = UtillGame.GetStraightHitPoint(_fireTransform.transform.position, inputVector, _maxDistance, _layerMask);
        photonView.RPC("UseOnServer", RpcTarget.All, playerViewID, endPoint);
    }
    protected virtual Projectile Pop()
    {
        var go = _projectilePoolSO.Pop();
        return go;
    }

    protected virtual IEnumerator ProcessAttackOnAllClinets(int playerViewID, Vector3 endPoint)
    {
        yield return new WaitForSeconds(0.3f);   //대미지 주기전까지 시간
        var projectile = Pop();

        ////맞은게 없다면 본인스스로 대미지. 시간30초아래일시 술래의 Health를 무적으로.
        projectile.transform.position = _fireTransform.position;
        projectile.Setup(Define.Team.Seek, playerViewID, _damage,_speed, endPoint);

        projectile.onPushEvent += () => _projectilePoolSO.Push(projectile);
    }
}
