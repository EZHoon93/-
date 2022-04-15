using Photon.Pun;
using UnityEngine;
public class PlayerHealth : LivingEntity
{
    //[SerializeField] statda
    [SerializeField] ParticleSystem _hitEffect;
    PlayerAvater _playerAvater;

    public override int currHp
    {
        get => base.currHp;
        set
        {
            //회복 이라면
            if(currHp < value)
            {
            }
            base.currHp = value;
            
        }
    }

    public override bool Dead { get => base.Dead;
        protected set
        {
            base.Dead = value;
            if (value)
            {
                //_playerStat.animator.SetTrigger("Die");
            }
        }
    }



    
    protected void Awake()
    {
        _playerAvater = GetComponent<PlayerAvater>();
    }

 

    [PunRPC]
    public override void OnDamage(int damagerViewId, int damage, Vector3 hitPoint)
    {
        base.OnDamage(damagerViewId, damage, hitPoint);
        if(this.ViewID() != damagerViewId)  //다른 플레이어의 공격 받을때만.
        {
            _hitEffect.transform.position = hitPoint;
            _hitEffect?.Play();
        }
        
    }
    [PunRPC]
    public override void Die()
    {
        //Managers.effectManager.EffectOnLocal(Define.EffectType.CloudBurst, this.transform.position, 0);
        base.Die();
        _playerAvater.PlayerAnimator.SetTrigger("Die");
    }

   
}
