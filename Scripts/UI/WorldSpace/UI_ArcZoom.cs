using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ArcZoom : UI_ZoomBase
{
    [SerializeField] float _angle;
    [SerializeField] Image _arcImage;
    [SerializeField] Transform _parentTr;

    [SerializeField]
    float _maxDistance;
    [SerializeField]
    float _range;
    Vector3 _inputVector3;

    private void Start()
    {
        _arcImage.fillAmount = _angle / 360;
        this.gameObject.SetActive(false);
    }
    public void ChangeMaxDistance(float newMaxDistance)
    {
        _maxDistance = newMaxDistance;
    }
    public void ChangeRange(float newRange)
    {
        _range = newRange;
    }
    public override void HandleDown(Vector3 inputVector3)
    {
        this.gameObject.SetActive(true);
        _parentTr.transform.localPosition = Vector3.zero;
    }
    public override void HandleDrag(Vector3 inputVector3)
    {
        _inputVector3 = inputVector3;
        _parentTr.transform.localPosition = inputVector3 * _maxDistance;
    }
    public override void HandleUp(Vector3 inputVector3)
    {
        this.gameObject.SetActive(false);
    }

 

}
