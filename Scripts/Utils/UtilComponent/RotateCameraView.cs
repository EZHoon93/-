using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCameraView : MonoBehaviour
{
    Camera _camera;
    private void Awake()
    {
        _camera = Camera.main;
    }
    private void LateUpdate()
    {
        this.transform.rotation = _camera.transform.rotation;
    }
}
