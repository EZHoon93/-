using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangeObject : MonoBehaviour
{



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            print("Test");
            var pv = PhotonView.Get(this);
            pv.RPC("Test", RpcTarget.All);
        }
    }

    [PunRPC]
    public void Test()
    {

        print("TTTTEST");
    }

}
