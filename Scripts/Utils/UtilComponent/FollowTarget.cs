using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] Vector3 _offset;
    private void LateUpdate()
    {
        this.transform.position = _target.transform.position + _offset;
    }
}
