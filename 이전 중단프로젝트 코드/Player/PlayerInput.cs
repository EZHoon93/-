using System.Collections.Generic;

using BehaviorDesigner.Runtime;

using Photon.Pun;

using UnityEngine;
using UnityEngine.AI;

public interface IAddInputInfo
{
    InputInfo AddInputInfo(InputDefine.InputType inputType, InputDefine.UIType uIType = InputDefine.UIType.Joystick, Sprite sprite = null);

     InputInfo GetInputInfo(InputDefine.InputType inputType);

}
public class PlayerInput : InputBase , IAddInputInfo , IOnPhotonInstantiate
{

    protected InputInfo _movenInputInfo = new InputInfo(InputDefine.InputType.Move);
    protected InputInfo _mainInputInfo = new InputInfo(InputDefine.InputType.Main);
    protected InputInfo _itemInputIfno1 = new InputInfo(InputDefine.InputType.Sub1);
    protected InputInfo _itemInputIfno2 = new InputInfo(InputDefine.InputType.Sub2);

    bool _isAI;
    public bool isTestGetInput = false;

    public Vector3 MoveVector3 => _movenInputInfo.inputVector3;
    public bool IsAI => _isAI;
    NavMeshAgent _navMeshAgent;
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void OnMyPhotonInstantiate(PhotonMessageInfo info)
    {
        _isAI = this.gameObject.IsValidAI();
        _navMeshAgent.enabled = this.IsMyCharacter();
        if (_isAI)
        {
            this.gameObject.GetOrAddComponent<AIPlayerController>();
        }
    }


    public override InputInfo GetInputInfo(InputDefine.InputType inputType)
    {
        switch (inputType)
        {
            case InputDefine.InputType.Move:
                return _movenInputInfo;
            case InputDefine.InputType.Main:
                return _mainInputInfo;
            case InputDefine.InputType.Sub1:
                return _itemInputIfno1;
            case InputDefine.InputType.Sub2:
                return _itemInputIfno2;
            default:
                break;
        }

        return null;
    }

    public override InputInfo AddInputInfo(InputDefine.InputType inputType, InputDefine.UIType uIType = InputDefine.UIType.Joystick, Sprite sprite = null)
    {
        var go = base.AddInputInfo(inputType, uIType, sprite);
        if(go != null && this.IsMyCharacter())
        {
            SetActiveControllerUI(go, true);
        }
        return go;
    }
    public override void RemoveInput(InputInfo inputInfo)
    {
        base.RemoveInput(inputInfo);
        SetActiveControllerUI(inputInfo, false);
    }

    private void Update()
    {
        if(photonView.IsMine == false)
        {
            return;
        }

        if (this.IsMyCharacter())
        {
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");
            var quaternion = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
            var direction = quaternion * new Vector3(x, 0, y).normalized;
            _movenInputInfo.inputVector3 = direction;
        }
        if (IsAI)
        {
            

            if (_navMeshAgent.enabled == false)
                return;
            _movenInputInfo.inputVector3 = _navMeshAgent.remainingDistance > 0.1f ? _navMeshAgent.velocity : Vector3.zero;
            if (isTestGetInput)
            {
                print(_movenInputInfo.inputVector3);
            }
        }
    }

   
}
