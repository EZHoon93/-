using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class InputDefine
{
    public enum InputTouchType
    {
        Down = 1,
        Drag,
        Up,
    }
    public enum InputType
    {
        Main,
        Item1,
        Item2
    }
    public enum UIType
    {
        Joystick = 1,
        Button,
        Toggle
    }
}
