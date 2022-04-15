using System;

using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(InputControllerObject))]

public abstract class Weapon : InputObjectBase, IPunObservable
{
    public delegate void WeaponUseHandler(Weapon weapon);

    #region Enum
    public enum WeaponType {
        Melee,
        Hammer,
        Throw,
        Gun,
        Bow ,
        TwoHandHammer,
        Projectile

    }
 
    #endregion

    public abstract WeaponType weaponType { get; }


 


    [SerializeField] protected int _damage;
    [SerializeField] protected Transform _model;
    [SerializeField] string _idleAnimName;
    [SerializeField] string _attackAnimName;



    public WeaponUseHandler onAttackStart;
    public WeaponUseHandler onAttackEnd;


    public string AttackAnimName => _attackAnimName;
    public string IdleAnimName => _idleAnimName;
    public float AttackDelay;
    public float AfaterAttackDelay;
    public float AttackDistance;
    public Vector3 attackPoint { get; protected set; }
    public Vector3 AttackInputVector3 { get; protected set; }


    #region Property



    #endregion


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
        }
        else
        {
       
        }
    }

    protected override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Resets();
        _playerController.PlayerShooter.SetupWeapon(this);
        //_playerController.playerShooter.
    }

    protected override void OnPhotonDestroy(PhotonView photonView)
    {
        onAttackStart = null;
        onAttackEnd = null;
    }


    protected virtual void Resets()
    {

    }
    public virtual void Attack(Vector3 inputVector3)
    {
    }
    //public virtual void Attack(Vector3 inputVector3, object[] datas)
    //{
    //    photonView.RPC("AttackOnServer", RpcTarget.AllViaServer, inputVector3);
    //    //var endPoint = GetHitPoint(_fireTransform.transform, inputVector3);
    //    //photonView.RPC("AttackOnServer", RpcTarget.AllViaServer, endPoint);
    //}

    //[PunRPC]
    //public virtual void AttackOnServer(Vector3 inputVector3)
    //{
    //    AttackInputVector3 = inputVector3;
    //}



    //public virtual void HandleAttackStart(Weapon weapon)
    //{

    //}

    //public virtual void HandleAttackEnd(Weapon weapon)
    //{

    //}






}
