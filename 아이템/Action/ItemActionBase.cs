using System.Collections;

using UnityEngine;


//[CreateAssetMenu(menuName = "Item/Action/ChangeObjectAction", fileName = "ChangeObjectAction ")]
public abstract class ItemActionBase : ScriptableObject
{
    //public abstract void UseOnServer(ItemSO itemSO);

    //public abstract IEnumerator UseOnServer(ItemSO itemSO);

    public abstract void UseOnServer(MyMonoBehaviourPun runner, ItemSO itemSO);
   
    public virtual void OnStartAction(MyMonoBehaviourPun runner,ItemSO itemSO)
    {
    }
    
    public virtual void OnDieAction(MyMonoBehaviourPun runner,ItemSO itemSO)
    {
    }
    public virtual void OnEndAction(MyMonoBehaviourPun runner,ItemSO itemSO)
    {
    }
}
