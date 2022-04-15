using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCollider : MonoBehaviour
{
    //NavMeshAgent _playerAgent;
    PlayerController _playerController;
    //IColliderEnter _colliderEnter;
    //PlayerController _playerController;
    Collider _currentCollider;

    private void Awake()
    {
        _playerController = this.transform.parent.GetComponent<PlayerController>();
        //_colliderEnter = this.transform.parent.GetComponent<IColliderEnter>();
        //_playerAgent = this.transform.parent.GetComponent<NavMeshAgent>();
        var playerHealth = this.transform.parent.GetComponent<PlayerHealth>();
        playerHealth.onChangeTeamEvent += HandleChangeTeam;

        HandleChangeTeam(playerHealth.Team);
    }

    void HandleChangeTeam(Define.Team team)
    {
        switch (team)
        {
            case Define.Team.Hide:
                this.gameObject.layer = (int)Define.Layer.Hider;
                break;
            case Define.Team.Seek:
                this.gameObject.layer = (int)Define.Layer.Seeker;
                break;
        }
    }

    /// <summary>
    /// Hider 모드에서 변신된  HideObject로 변신된 콜라이더 값을 복사
    /// </summary>
    /// <param name="changeTarget"></param> 변신된 오브젝트
    public void ChangeCollider(NavMeshObstacle navMeshObstacle)
    {
        if (_currentCollider)
        {
            Destroy((Component)(_currentCollider));
        }
        if(navMeshObstacle.shape == NavMeshObstacleShape.Capsule)
        {
            var capsuleCollider= this.gameObject.GetOrAddComponent<CapsuleCollider>();
            capsuleCollider.center = navMeshObstacle.center;
            capsuleCollider.radius = navMeshObstacle.radius;
            _currentCollider = capsuleCollider;
        }
        else
        {
            var boxCollider= this.gameObject.GetOrAddComponent<BoxCollider>();
            boxCollider.center = navMeshObstacle.center;
            boxCollider.size = navMeshObstacle.size;
            _currentCollider = boxCollider;
        }
        _currentCollider.isTrigger = true;
        _currentCollider.enabled = true ;

    }

    private void OnTriggerEnter(Collider other)
    {
        //_colliderEnter.OnCallBackTriggerEnter(other);
        var enterTrigger = other.gameObject.GetComponent<ICanEnterTriggerPlayer>();
        if (enterTrigger != null)
        {
            enterTrigger.Enter(_playerController, other);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        var exitTrigger = other.GetComponent<ICanExitTriggerPlayer>();
        if (exitTrigger != null)
        {
            exitTrigger.Exit(_playerController, other);
        }
    }



}


