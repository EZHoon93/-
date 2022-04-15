using System.Collections;
using System.Collections.Generic;

using Photon.Pun;

using UnityEngine;

public abstract class ObtainItemBase : MonoBehaviourPun , IInputObject
{
    [SerializeField] GameObject _modelObject;
    public GameObject ModelObject => _modelObject;

    public abstract void Use(int playerViewID, Vector3 inputVector);

    
}
