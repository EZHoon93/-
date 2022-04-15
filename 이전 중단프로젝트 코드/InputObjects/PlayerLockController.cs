using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLockController : InputObjectBase
{
    bool _isLock;

    protected override void Init()
    {
    }
    private void OnEnable()
    {
        _isLock = false;
    }
    protected override void CallBackDown(Vector3 inputVector3)
    {

    }


}
