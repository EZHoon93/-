using UnityEngine;

public abstract class UI_ZoomBase : MonoBehaviour
{
    [SerializeField] InputReader _inputReader;
    [SerializeField] private PhotonEventChannelSO _playerControllerChangeEventSO;   //사용자 
    protected GameObject _inputControllerObject;
    private InputDefine.InputType _inputType; 
 
    //public virtual void Setup(InputControllerObject inputControllerObject)
    //{

    //    _inputType = inputControllerObject.InputType;
    //    var inputInfoSO = _inputReader.GetInputInfoSO(_inputType);
    //    inputInfoSO.onDownEvent += HandleDown;
    //    inputInfoSO.onDragEvent += HandleDrag;
    //    inputInfoSO.onUpEvent += HandleUp;
    //    this.transform.ResetTransform(inputControllerObject.transform);
    //}
    public virtual void Setup(InputDefine.InputType inputType , GameObject inputControllerObject)
    {
        _inputControllerObject = inputControllerObject;
        var inputInfoSO = _inputReader.GetInputInfoSO(_inputType);
        inputInfoSO.onDownEvent += HandleDown;
        inputInfoSO.onDragEvent += HandleDrag;
        inputInfoSO.onUpEvent += HandleUp;
    }
    protected virtual void OnDestroy()
    {
        print("OnDestroy  UI_ZoomBase!!");
        var inputInfoSO = _inputReader.GetInputInfoSO(_inputType);
        inputInfoSO.onDownEvent -= HandleDown;
        inputInfoSO.onDragEvent  -= HandleDrag;
        inputInfoSO.onUpEvent -= HandleUp;
    }


    public virtual void HandleDown(Vector3 inputVector3)
    {

    }
    public virtual void HandleDrag(Vector3 inputVector3)
    {

    }
    public virtual void HandleUp(Vector3 inputVector3)
    {

    }


}
