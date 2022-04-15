using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : Movement , IOnPhotonInstantiate
{
    


    PlayerInput _playerInput;
    PlayerShooter _playerShooter;
    NavMeshAgent _navMeshAgent;
    bool _isAI;

    [SerializeField]
    private InputDefine.InputType _moveInputType;
    [SerializeField]
    private InputDefine.InputType _rotateInputType;
    
    [SerializeField]
    private bool _notRotate;

    private Vector3 MoveVector3 => _playerInput.GetInputInfo(_moveInputType).inputVector3.normalized;
    private Vector3 RotateVector3 => _playerInput.GetInputInfo(_rotateInputType).inputVector3;

    public override float MoveSpeed { 
        get => base.MoveSpeed;
        set
        {
            _moveSpeed = value;
            onChangeMoveSpeed?.Invoke(value);
            _navMeshAgent.speed = value;
        }
            
    }


    #region CallBack
    public void OnMyPhotonInstantiate(PhotonMessageInfo info)
    {
        _isAI = this.gameObject.IsValidAI();
    }
    #endregion

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerShooter = GetComponent<PlayerShooter>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.enabled = true;
        _playerInput.AddInputInfo(_moveInputType);
        _playerInput.AddInputInfo(_rotateInputType);
    }
  

    private void FixedUpdate()
    {
        //로컬만 실행
        if(!photonView.IsMine)
        {
            return;
        }
        switch (_playerShooter.ShooterState)
        {
            case PlayerShooter.STATE.Idle:
                UpdateRotate(RotateVector3 , _rotateSpeed);
                UpdateMove();
                break;
            case PlayerShooter.STATE.OnlyRotate:
                UpdateRotate(_playerShooter.Direction , _rotateSpeed * 2);
                break;
            case PlayerShooter.STATE.Skill:
                break;
        }
    }

    protected virtual void UpdateMove( )
    {
        //조이스틱 입력안할시
        if (_playerInput.MoveVector3.sqrMagnitude == 0)
        {
            MoveState = State.Idle;
            return;
        }
        //뛰기 버튼 시 
        MoveState = State.Run;
        if (_navMeshAgent.enabled && !_isAI)
        {
            Vector3 moveDistance = MoveVector3 * _moveSpeed * Time.deltaTime;
            _navMeshAgent.Move(moveDistance);
        }

    }

    protected virtual void UpdateRotate( Vector3 inputVector3 , float rotateSpeed)
    {
        if (inputVector3.sqrMagnitude == 0 || _notRotate)
        {
            return;
        }
        Quaternion newRotation = Quaternion.LookRotation(inputVector3);
        //var ss = Quaternion.Slerp(this.transform.rotation, newRotation, _rotateSpeed * Time.deltaTime);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, newRotation, _rotateSpeed * Time.deltaTime);
        //float turn = _playerInput.MoveVector3* 5 * Time.deltaTime;
        //_rigidbody.rotation =_rigidbody.rotation * Quaternion.Euler(0, 3, 0f);

        //_rigidbody.rotation = Quaternion.Slerp(this.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        //navMeshAgent.updateRotation
        //_rigidbody.MoveRotation(ss);
        
    }

  
    //void UpdateMoveAnimation()
    //{
    //    _playerAvater.UpdateAnimatorMove(MoveVector3.sqrMagnitude);

    //}


}
