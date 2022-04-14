using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedRotation : MonoBehaviour
{
    [SerializeField] private Vector3 _vector3;

    
    private void LateUpdate()
    {
        this.transform.rotation = Quaternion.Euler(_vector3);
    }
}
