using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Data;


public class ProjectileWeapon : Weapon
{
    public override WeaponType weaponType => WeaponType.Throw;

    [SerializeField] GameObject _projectilePrefab;
    [SerializeField] Transform _fireTransform;
    [SerializeField] float _initMaxDistance;
    [SerializeField] float _initRange;



    Stack<Projectile> _projectileList = new Stack<Projectile>();
    int _weaponSkinKey;
    int _initProjectileCount = 3;
    float _maxDistance;
    float _currentRange;


    public floatEvent onChangeDistance;
    public floatEvent onChangeRange;


    public float MaxDistance
    {
        get => _maxDistance;
        set
        {
            _maxDistance = value;
            onChangeDistance?.Invoke(value);
        }
    }

    public float CurrentWidth
    {
        get => _currentRange;
        set
        {
            _currentRange = value;
            onChangeRange?.Invoke(value);

        }
    }
    protected override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        base.OnPhotonInstantiate(info);
        var HT = (Hashtable)info.photonView.InstantiationData[1];
        _weaponSkinKey = (int)HT["we"];
        CreateProjectile(_initProjectileCount);
    }
    protected override void Resets()
    {
        MaxDistance = _initMaxDistance;
        CurrentWidth = _initRange;
    }

    protected override void CallBackUp(Vector3 inputVector3)
    {
        if (inputVector3.sqrMagnitude == 0 || _playerController.PlayerShooter.ShooterState != PlayerShooter.STATE.Idle)
        {
            return;
        }
        if (_inputControllerObject.Use())
        {
            Attack(inputVector3);
        }
    }
    public override void Attack(Vector3 inputVector3)
    {
        var endPoint = this.transform.position + inputVector3 * MaxDistance;
        photonView.RPC("AttackOnServer", RpcTarget.AllViaServer, inputVector3, endPoint);
    }
    [PunRPC]
    public void AttackOnServer(Vector3 inputVector3, Vector3 endPoint, PhotonMessageInfo photonMessageInfo)
    {
        AttackInputVector3 = inputVector3;
        StartCoroutine(ProcessAttackOnAllClinets(endPoint));
    }

    IEnumerator ProcessAttackOnAllClinets(Vector3 endPoint)
    {
        onAttackStart?.Invoke(this);
        yield return new WaitForSeconds(AttackDelay);   //대미지 주기전까지 시간
        _model.gameObject.SetActive(false);
        var projectile = Pop();
        var playerViewID = _playerController.ViewID();
        //맞은게 없다면 본인스스로 대미지. 시간30초아래일시 술래의 Health를 무적으로.
        projectile.Setup(playerViewID, _playerController.Team, _damage, _fireTransform.position, endPoint);
        yield return new WaitForSeconds(AfaterAttackDelay);   //대미지 주기전까지 시간
        onAttackEnd?.Invoke(this);
        _model.gameObject.SetActive(true);
    }


    void CreateProjectile(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var projectile = Instantiate(_projectilePrefab.gameObject).GetComponent<Projectile>();
            //projectile.Setup(this, _weaponSkinKey);
            projectile.gameObject.SetActive(false);
            _projectileList.Push(projectile);
        }
    }

    Projectile Pop()
    {
        if (_projectileList.Count <= 0)
        {
            CreateProjectile((int)(_initProjectileCount * .5f));
        }
        var go = _projectileList.Pop();
        go.gameObject.SetActive(true);
        return go;
    }

    public void Push(SeekerProjectile seekerProjectile)
    {
        seekerProjectile.gameObject.SetActive(false);
        _projectileList.Push(seekerProjectile);
    }
}
