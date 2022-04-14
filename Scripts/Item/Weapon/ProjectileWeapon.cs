using System.Collections;
using UnityEngine;
using Photon.Pun;
using Data;

public class ProjectileWeapon : Weapon
{

    //[SerializeField] protected Projectile _projectilePrefab;
    //[SerializeField] protected Transform _fireTransform;

    //[SerializeField] float _initMaxDistance;
    //[SerializeField] float _initRange;

    //float _maxDistance;
    //float _currentRange;
    //public floatEvent onChangeDistance;
    //public floatEvent onChangeRange;
    //public float MaxDistance
    //{
    //    get => _maxDistance;
    //    set
    //    {
    //        _maxDistance = value;
    //        onChangeDistance?.Invoke(value);
    //    }
    //}

    //public float CurrentWidth
    //{
    //    get => _currentRange;
    //    set
    //    {
    //        _currentRange = value;
    //        onChangeRange?.Invoke(value);

    //    }
    //}

    //protected override void Resets()
    //{
    //    MaxDistance = _initMaxDistance;
    //    CurrentWidth = _initRange;
    //}

    //protected override void CallBackUp(Vector3 inputVector3)
    //{
    //    photonView.RPC("UseOnServer", RpcTarget.AllViaServer, inputVector3, endPoint);
    //}
 
    //[PunRPC]
    //public void UseOnServer(Vector3 inputVector3, Vector3 endPoint, PhotonMessageInfo photonMessageInfo)
    //{
    //    AttackInputVector3 = inputVector3;
    //    //StartCoroutine(ProcessAttackOnAllClinets(endPoint));
    //}

    //protected virtual IEnumerator ProcessAttackOnAllClinets(Vector3 endPoint)
    //{
    //    //onAttackStart?.Invoke(this);
    //    //yield return new WaitForSeconds(AttackDelay);   //대미지 주기전까지 시간
    //    //_model.gameObject.SetActive(false);
    //    //var projectile = Pop();
    //    //var playerViewID = _playerController.ViewID();
    //    ////맞은게 없다면 본인스스로 대미지. 시간30초아래일시 술래의 Health를 무적으로.
    //    ////projectile.Setup(playerViewID, _playerController.Team, _damage, _fireTransform.position, endPoint);
    //    //yield return new WaitForSeconds(AfaterAttackDelay);   //대미지 주기전까지 시간
    //    //onAttackEnd?.Invoke(this);
    //    //_model.gameObject.SetActive(true);
    //}

    //protected virtual Projectile Pop()
    //{
    //    //var go = Managers.Pool.Pop(_projectilePrefab.gameObject).GetComponent<Projectile>();

    //    return null;
    //}

}
