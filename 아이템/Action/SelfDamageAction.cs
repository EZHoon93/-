
using Sirenix.OdinInspector;

using UnityEngine;


[CreateAssetMenu(menuName = "Item/Action/SelfDamage", fileName = "ItemAction SelfDamage")]
public class SelfDamageAction : SerializedScriptableObject
{
    [SerializeField] private int _selfDamage;
    public void OnSelfDamage(ItemSO itemSO, int count)
    {
        if (count != 0)
            return;
        var player = itemSO.PlayerController;
        if (player == null)
            return;
        if(player.TryGetComponent(out Damageable damageable))
        {
            damageable.ReceiveAnAttack(player.photonView.ViewID, _selfDamage, player.transform.position);
        }

    }

    //public void OnTest(Dama)
}
