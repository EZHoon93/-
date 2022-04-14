using System.Collections;
using System.Collections.Generic;

using Photon.Pun;

using UnityEngine;

public class HealItem : ObtainItemBase
{
    [SerializeField]
    int _recoveryHealAmount;
    [Header("BroadCasting")]
    [SerializeField] private EachIntEventChannelSO _healEvent;

    public override void Use(int playerViewID, Vector3 inputVector)
    {
        _healEvent.RaiseEvent(playerViewID, _recoveryHealAmount);
        Invoke("AfterDestroy", 1.0f);
    }

    private void AfterDestroy()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

}
