using System.Collections;
using System.Collections.Generic;

using Photon.Pun;

using UnityEngine;

public abstract class ObtainItemBase : InputObjectBase 
{
    [SerializeField] float _needTime;
    [SerializeField] GameObject _modelObject;

    public GameObject ModelObject => _modelObject;
    public float GetNeedTime { 
        get => _needTime;
        set
        {
            _needTime = value;
        }
    }

    protected abstract void Use();
    
}
