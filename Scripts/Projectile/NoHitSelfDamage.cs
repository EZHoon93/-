using UnityEngine;

public class NoHitSelfDamage : MonoBehaviour
{
    [SerializeField] PlayerRuntimeSet _playerRuntimeSet;
    [SerializeField] int _damage;

    MissileProjectile _projectile;
    private void Awake()
    {
        _projectile =  GetComponent<MissileProjectile>();
    }


    /// <summary>
    /// ¾Æ¹«µµ ¸ø¸Â­ŸÀ»¶§ ´ë¹ÌÁö¸¦ ÁÜ
    /// </summary>
    public void OnDamage(int count)
    {
        if (count != 0)
            return;

        //0 ÀÌ¸é ´øÁøÇÃ·¹ÀÌ¾î¿¡°Ô ´ë¹ÌÁö.
        var playerViewID = _projectile.PlayerViewID;
        var playerController = _playerRuntimeSet.GetItem(playerViewID);
        if (playerController == null)
        {
            return;
        }
        if (playerController.TryGetComponent(out Damageable damageable))
        {
            damageable.ReceiveAnAttack(playerViewID, _damage, playerController.transform.position);
        }
    }
}
