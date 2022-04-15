using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : ObtainItemBase
{
    [SerializeField] ParticleSystem _effect;
    protected override void CallBackDown(Vector3 inputVector3)
    {
        Use();
    }
    protected override void Use()
    {
        //Managers.Resource.PunDestroy(this);
        _playerController.PlayerShooter.RemoveInputControllerObject(InputControllerObject);
        Managers.effectManager.EffectToServer(Define.EffectType.Heal, _playerController.transform.position, 0);
    }
}
