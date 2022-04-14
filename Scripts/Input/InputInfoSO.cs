using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu (menuName = "Input/InputInfo")]
public class InputInfoSO : DescriptionBaseSO
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private int _fireCode;



    public event UnityAction<Vector3> onDownEvent;
    public event UnityAction<Vector3> onDragEvent;
    public event UnityAction<Vector3> onUpEvent;

    private UIController _uI_Controller;
    public Vector3 inputVector3;
    public bool IsUsing;

    public void Setup(UIController uIController)
    {
        _uI_Controller = uIController;
        _uI_Controller.gameObject.SetActive(false);
    }

    public void AddInputInfo(ItemSO itemSO = null)
    {
        _uI_Controller.SetupItem(itemSO);
        _uI_Controller.gameObject.SetActive(true);
    }

    public void RemoveInputInfo()
    {
        _uI_Controller.gameObject.SetActive(false);
    }

    public void Clear()
    {
        onDownEvent = null;
        onDragEvent = null;
        onUpEvent = null;
    }


    public void OnDown(Vector3 vector3)
    {
        onDownEvent?.Invoke(vector3);
    }
    public void OnDrag(Vector3 vector3)
    {
        onDragEvent?.Invoke(vector3);
    }
    public void OnUp(Vector3 vector3)
    {
        if(_inputReader)
            _inputReader.Fire(_fireCode, vector3);
        onUpEvent?.Invoke(vector3);
    }
}
