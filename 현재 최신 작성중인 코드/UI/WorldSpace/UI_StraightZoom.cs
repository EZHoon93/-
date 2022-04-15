

using UnityEngine;

public class UI_StraightZoom : UI_ZoomBase
{

    #region Varaibles
    [SerializeField] private LayerMask _layer;
    [SerializeField] private float _startRatio;
    [SerializeField] private float _height;
    [SerializeField] LineRenderer _lineRenderer;

    float _distance;
    float _range;
    Vector3 _inputVector3;
    #endregion
    #region Properties
    #endregion

    #region Life Cycle
    private void Start()
    {
        _lineRenderer.positionCount = 2;
        _lineRenderer.enabled = false;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (_inputControllerObject.TryGetComponent(out Weapon weapon))
        {
            weapon.onChangeDistance -= OnChangeDistance;
            weapon.onChangeRange -= OnChangeRange;
        }
    }

    private void LateUpdate()
    {
        if (_inputVector3.sqrMagnitude == 0)
        {
            _lineRenderer.enabled = false;
            return;
        }

        var startPoint = this.transform.position + _inputVector3.normalized * _startRatio;
        var endPoint = UtillGame.GetStraightHitPoint(startPoint, _inputVector3, _distance, _layer);
        startPoint.y = _height;
        endPoint.y = _height;
        _lineRenderer.SetPosition(0, startPoint);
        _lineRenderer.SetPosition(1, endPoint);
        _lineRenderer.enabled = true;
    }
    #endregion

    #region public
    public override void Setup(InputDefine.InputType inputType, GameObject inputControllerObject)
    {
        base.Setup(inputType, inputControllerObject);
        if (inputControllerObject.TryGetComponent(out Weapon weapon))
        {
            weapon.onChangeDistance += OnChangeDistance;
            weapon.onChangeRange += OnChangeRange;
            weapon.onDestroyEvent += OnWeaponDisable;
            OnChangeDistance(weapon.MaxDistance);
            OnChangeRange(weapon.MaxRange);
        }
    }

    public override void HandleDown(Vector3 inputVector3)
    {
        this.gameObject.SetActive(true);
        _inputVector3 = Vector3.zero;
    }
    public override void HandleDrag(Vector3 inputVector3)
    {
        _inputVector3 = inputVector3;
    }
    public override void HandleUp(Vector3 inputVector3)
    {
        this.gameObject.SetActive(false);
        _inputVector3 = Vector3.zero;
    }

    #endregion

    #region CallBack
    private void OnWeaponDisable()
    {
        Destroy(this.gameObject);
    }

    private void OnChangeRange(float newRange)
    {
        _range = newRange;
        _lineRenderer.startWidth = _range;
        _lineRenderer.endWidth = _range;
    }

    private void OnChangeDistance(float newDistance)
    {
        _distance = newDistance;
    }
    #endregion

    #region private


    #endregion






}
   
    
   
