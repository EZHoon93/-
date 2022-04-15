using UnityEngine;
using Photon.Pun;
[RequireComponent(typeof(InputControllerObject))]
public abstract class InputObjectBase : MonoBehaviourPun
{
    protected InputControllerObject _inputControllerObject;
    protected PlayerController _playerController => _inputControllerObject.PlayerController;


    public InputControllerObject InputControllerObject => _inputControllerObject;

    private void Awake()
    {
        _inputControllerObject = GetComponent<InputControllerObject>();
        _inputControllerObject.downHandlerCallBack += CallBackDown;
        _inputControllerObject.dragHandlerCallBack += CallBackDrag;
        _inputControllerObject.upHandlerCallBack += CallBackUp;
        _inputControllerObject.instinateCallBack += OnPhotonInstantiate;
        _inputControllerObject.destroyCallBack += OnPhotonDestroy;
        Init();
    }

    protected virtual void Init()
    {

    }
    protected virtual void OnPhotonInstantiate(PhotonMessageInfo info)
    {

    }

    protected virtual void OnPhotonDestroy(PhotonView photonView)
    {

    }

    protected virtual void CallBackDown(Vector3 inputVector3)
    {
    }
    protected virtual void CallBackDrag(Vector3 inputVector3)
    {
    }
    protected virtual void CallBackUp(Vector3 inputVector3)
    {
    }
}
