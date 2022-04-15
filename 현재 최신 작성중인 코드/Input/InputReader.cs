using UnityEngine;
using UnityEngine.Events;



public delegate void InputCallBak(int fireCode, Vector3 inputVector);
[System.Serializable]
public class InputReader : DescriptionBaseSO
{
    [SerializeField] private InputInfoSO _moveInputInfoSO;
    [SerializeField] private InputInfoSO _mainInputInfoSO;
    [SerializeField] private InputInfoSO _itemInputInfoSO1;
    [SerializeField] private InputInfoSO _itemInputInfoSO2;
    public InputCallBak onInputFireEvent;

    public Vector3 MoveInputVector3
    {
        get
        {
            return _moveInputInfoSO.inputVector3;

            //#if UNITY_EDITOR
            //            var x = Input.GetAxis("Horizontal");
            //            var y = Input.GetAxis("Vertical");
            //            return new Vector3(x, 0, y);
            //#endif
#if UNITY_ANDROID
            return _moveInputInfoSO.inputVector3;
#endif
        }
    }

    public void Fire(int index , Vector3 inputVector)
    {
        onInputFireEvent?.Invoke(index, inputVector);
    }

    public void SetActiveMoveVector(bool active)
    {
        Debug.Log(active + "¹«ºêº¤ÅÍ");
        if (active)
            _moveInputInfoSO.AddInputInfo();
        else
            _moveInputInfoSO.RemoveInputInfo();
    }
  
    public InputInfoSO GetInputInfoSO(InputDefine.InputType inputType)
    {
        InputInfoSO inputInfoSO = null;
        switch (inputType)
        {
            case InputDefine.InputType.Main:
                inputInfoSO = _mainInputInfoSO;
                break;
            case InputDefine.InputType.Item1:
                inputInfoSO = _itemInputInfoSO1;
                break;
            case InputDefine.InputType.Item2:
                inputInfoSO = _itemInputInfoSO2;
                break;
        }
        return inputInfoSO;
    }


    public void AddUIController(int fireCode, ItemSO itemSO = null)
    {
        var inputType = (InputDefine.InputType)fireCode;
        var inputInfo = GetInputInfoSO(inputType);
        inputInfo.AddInputInfo(itemSO);
        Debug.Log("Add UICOntroller");

    }
    public void RevmoeUIController(int fireCode)
    {
        var inputType = (InputDefine.InputType)fireCode;
        var inputInfo = GetInputInfoSO(inputType);
        inputInfo.RemoveInputInfo();
    }



}
