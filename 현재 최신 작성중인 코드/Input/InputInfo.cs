using System.Collections.Generic;
using UnityEngine;



public delegate void InputHandler(Vector3 inputVector3);

public class InputInfo 
{
    public InputDefine.UIType UIType{ get; set; }
    public InputDefine.InputType InputType { get; set; }

    public Sprite Sprite { get; set; }
    public Vector3 inputVector3 { get; set; }
    public float coolTime { get; set; }
    public float remainCoolTime { get; set; }
    public bool IsUsing { get; set; }

    public InputHandler drageHanlder;
    public InputHandler upHanlder;
    public InputHandler downHanlder;



    public InputInfo(InputDefine.InputType inputType, InputDefine.UIType uIType = InputDefine.UIType.Joystick)
    {
        InputType = inputType;
        UIType = uIType;
        IsUsing = false;
    }


    public void Reset()
    {
        inputVector3 = Vector2.zero; ;
        remainCoolTime = 0;

        drageHanlder = null;
    }
}
