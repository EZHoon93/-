
using Photon.Pun;

using UnityEngine;

public class PlayerInput : MonoBehaviourPun , IPunInstantiateMagicCallback
{
    [SerializeField] private InputReader _inputReader;
    public Vector3 MoveVector { get; private set; }

    public Vector3 AttackVector { get; private set; }

    public InputCallBak onInputFireEvent;


    private void Awake()
    {
        
    }
    
    private void OnDisable()
    {
        _inputReader.onInputFireEvent -= Fire;
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        var controllerNr  = (int)info.photonView.InstantiationData[0];
        if(PhotonNetwork.LocalPlayer.ActorNumber == controllerNr)
        {
            LocalUserInputReaderSetup();
        }
    }

    private void LocalUserInputReaderSetup() 
    {
        _inputReader.onInputFireEvent += Fire;
        _inputReader.SetActiveMoveVector(true);
    }


    private void Update()
    {
        if (photonView.IsMine == false)
        {
            return;
        }

        if (this.gameObject.IsValidAI())
        {

        }
        else
        {
            MoveVector = _inputReader.MoveInputVector3;
        }
    }

    public void Fire(int fireCode, Vector3 inputVector)
    {
        onInputFireEvent?.Invoke(fireCode, inputVector);
    }
}
