using System.Collections;

using UnityEngine;



[CreateAssetMenu(menuName = "Item/Action/RecoveryHealthAction", fileName = "ItemAction ")]
public class RecoveryHealthAction : ItemActionBase
{
    [SerializeField] private int _amount;
    [SerializeField] private IntEventChannelSO _onRecoveryEventSO;

    //public override IEnumerator Sequence()
    //{
    //    throw new System.NotImplementedException();
    //}

    //public override IEnumerator UseOnServer(ItemSO itemSO)
    //{
    //    _onRecoveryEventSO.RaiseEvent(_amount, itemSO.PlayerViewID);
    //    yield return null;
    //}

    public override void UseOnServer(MyMonoBehaviourPun runner, ItemSO itemSO)
    {

    }


}
