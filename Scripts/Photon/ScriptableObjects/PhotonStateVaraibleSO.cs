using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PhotonState
{
    DisConnect,
    Connecting,
    Connect
}

[CreateAssetMenu(menuName = "Photon/Varaibles/PhotonState")]
public class PhotonStateVaraibleSO : VariableSO<PhotonState>
{

}
