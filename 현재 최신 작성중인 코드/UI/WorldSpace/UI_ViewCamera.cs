using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ViewCamera : MonoBehaviour
{
    Transform _camera;
    private void Start()
    {
        _camera = Camera.main.transform;
    }
    private void LateUpdate()
    {
        this.transform.rotation = _camera.rotation;
    }

}
