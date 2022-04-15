
using System.Collections.Generic;

using Photon.Pun;

using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerShooter : MonoBehaviourPun
{
    public enum STATE
    {
        Idle,    //자유로운상태
        UpperRotate,    //움직일수있음, 상체만로테이샨
        OnlyRotate, //움직일수없음, 전체로테이션
        Skill   //스킬, 서버로 rpc로 실행, 동시시작
    }
    public STATE ShooterState { get; set; }

    InputControllerObject _currentInputControllerObject;
    PlayerInput _playerInput;
    PlayerAvater _playerAvater;
    Dictionary<InputDefine.InputType, InputControllerObject> _inputConrollerDic = new Dictionary<InputDefine.InputType, InputControllerObject>();


    public Animator _playerAnimator => _playerAvater.PlayerAnimator;
    public Vector3 Direction { get; protected set; }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerAvater = GetComponent<PlayerAvater>();
    }

    private void OnEnable()
    {
        ShooterState = STATE.Idle;
    }

    /// <summary>
    /// 새로운것 등록, 기존것은 없애버림. 한컨트롤러당 1개의 inputControllerObject
    /// </summary>
    public void SetupInputControllerObject(InputControllerObject newInputControllerObject)
    {
        var inputType = newInputControllerObject.InputType;
        var uiType = newInputControllerObject.UIType;
        var sprite = newInputControllerObject.sprite;
        //기존거 없앰
        if (_inputConrollerDic.ContainsKey(inputType))
        {
            var prevControllerObject = _inputConrollerDic[inputType];   //기존거..

            _inputConrollerDic[inputType] = newInputControllerObject;
        }
        //없으면 등록
        else
        {
            _inputConrollerDic.Add(newInputControllerObject.InputType, newInputControllerObject);
        }
        var inputInfo = _playerInput.AddInputInfo(inputType,uiType,sprite);
        inputInfo.downHanlder += newInputControllerObject.Down;
        inputInfo.drageHanlder += newInputControllerObject.Drag;
        inputInfo.upHanlder += newInputControllerObject.Up;
        
    }

    public void RemoveInputControllerObject(InputControllerObject inputControllerObject)
    {
        var inputType = inputControllerObject.InputType;
        if (_inputConrollerDic.ContainsKey(inputType))
        {
            _inputConrollerDic[inputType] = null;
            var inputInfo = _playerInput.GetInputInfo(inputType);
            _playerInput.RemoveInput(inputInfo);
        }
    }
    public void SetupWeapon(Weapon newWeapon)
    {
        newWeapon.onAttackStart = WeaponStart;
        newWeapon.onAttackEnd = WeaponEnd;
        if(string.IsNullOrEmpty(newWeapon.IdleAnimName) == false)
        {
            _playerAnimator.SetTrigger(newWeapon.IdleAnimName);
        }
    }
    void WeaponStart(Weapon weapon)
    {
        _playerAnimator.SetTrigger("Attack");
        ShooterState = weapon.InputControllerObject.ShooterState;
        Direction = weapon.AttackInputVector3;
    }

    void WeaponEnd(Weapon weapon)
    {
        ShooterState = STATE.Idle;
    }

   
    private void OnAnimatorIK(int layerIndex)
    {
        print("IK122");

    }
}
