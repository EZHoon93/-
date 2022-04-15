using System.Collections;
using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using Data;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class Weapon_Seeker : Weapon
{
    public override WeaponType weaponType => WeaponType.Projectile;
    [SerializeField] GameObject _projectilePrefab;
    [SerializeField] Transform _fireTransform;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] float _initMaxDistance;
    [SerializeField] float _initWidth;
    [SerializeField] int _noHitDamage = 10;

    public floatEvent onChangeDistance;
    public floatEvent onChangeWidth;

    Stack<SeekerProjectile> _projectileList = new Stack<SeekerProjectile>();
    int _initProjectileCount = 3;
    int _weaponSkinKey;

    private float _maxDistance;

    public float MaxDistance
    {
        get => _maxDistance;
        set
        {
            _maxDistance = value;
            onChangeDistance?.Invoke(value );
        }
    }

    private float _currentWidth;
    public float CurrentWidth
    {
        get => _currentWidth;
        set
        {
            _currentWidth = value;
            onChangeWidth?.Invoke(value );
            
        }
    }

    protected override void Init()
    {

    }
    protected override void Resets()
    {
        MaxDistance = _initMaxDistance;
        CurrentWidth = _initWidth;
    }

    protected override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        base.OnPhotonInstantiate(info);
        var HT = (Hashtable)info.photonView.InstantiationData[1];
        _weaponSkinKey = (int)HT["we"];
        CreateProjectile(_initProjectileCount);
    }

    protected override void OnPhotonDestroy(PhotonView photonView)
    {
        foreach(var projectile in _projectileList.ToArray())
        {
            Managers.Resource.Destroy(projectile.gameObject);
        }
        _projectileList.Clear();
    }

  
    protected override void CallBackUp(Vector3 inputVector3) 
    {
        if(inputVector3.sqrMagnitude    == 0 ||  _playerController.PlayerShooter.ShooterState != PlayerShooter.STATE.Idle)
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
        var endPoint = GetHitPoint(_fireTransform.transform,  inputVector3);
        photonView.RPC("AttackOnServer", RpcTarget.AllViaServer, inputVector3, endPoint);
    }

    [PunRPC]
    public void AttackOnServer(Vector3 inputVector3, Vector3 endPoint , PhotonMessageInfo photonMessageInfo)
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
        projectile.Setup(playerViewID, _playerController.Team, _damage, _fireTransform.position, endPoint , () =>
        {
            _playerController.PlayerHealth.OnDamage(playerViewID, _noHitDamage, Vector3.zero);
        });
        yield return new WaitForSeconds(AfaterAttackDelay);   //대미지 주기전까지 시간
        onAttackEnd?.Invoke(this);
        _model.gameObject.SetActive(true);
    }

    void CreateProjectile(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var projectile = Instantiate(_projectilePrefab.gameObject).GetComponent<SeekerProjectile>();
            //projectile.Setup(this, _weaponSkinKey);
            projectile.gameObject.SetActive(false);
            _projectileList.Push(projectile);
        }
    }

    SeekerProjectile Pop()
    {
        if(_projectileList.Count <= 0)
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

    Vector3 GetHitPoint(Transform rayTransform, Vector3 inputVector3)
    {
        RaycastHit hit;
        Vector3 hitPosition;
        Vector3 start = rayTransform.transform.position;
        start.y = 0.5f;
        Vector3 direction = inputVector3.normalized;
        if (Physics.Raycast(start, direction, out hit, _maxDistance, _layerMask))
        {
            hitPosition = hit.point;
            hitPosition.y = rayTransform.transform.position.y;
        }
        else
        {
            hitPosition = start + direction * _maxDistance;

        }
        hitPosition.y = 0.5f;
        return hitPosition;
    }

    
}
