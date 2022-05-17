using System.Collections.Generic;

using Beautify.Universal;

using FoW;

using Photon.Pun;

using UnityEngine;


[CreateAssetMenu(menuName = "Buff/Action/Sleep")]

public class SleepAction : BuffFactoryBase
{
    //public override List<PlayerController> Damageables { get => base.Damageables; set => base.Damageables = value; }
    [SerializeField] private BoolEventChannelSO _cameraCurseEventSO;
    //[SerializeField] private int _blindValue;
    [SerializeField] private FloatVaraibleSO _blindValueSO;
    private float _originValue;
    //FogOfWarUnit _fogOfWarUnit;
    //Damageable _damageable;

    public float Blind => _blindValueSO.Value;
    
    
    public void SetupData(float blindValue)
    {

    }

    //데이터는 참조만 , 
    public override object Clone()
    {
        var copy = CreateInstance<SleepAction>();
        //copy._blindValue = ;
        return copy;
    }

    protected override void StartAction()
    {
        
    }

    protected override void EndAction()
    {
        
    }



    //public override void OnStart()
    //{
    //    //if (_target == null)
    //    //{
    //    //    OnEnd();
    //    //    return;
    //    //}

    //    //if( _target.photonView.ControllerActorNr == PhotonNetwork.LocalPlayer.ActorNumber)
    //    //{
    //    //    _originValue = BeautifySettings.settings.vignettingFade.value;
    //    //    BeautifySettings.settings.vignettingFade.value = .95f;
    //    //}
    //    //_fogOfWarUnit = _target.GetComponent<FogOfWarUnit>();
    //    //_damageable = _target.GetComponent<Damageable>();
    //    //if (_fogOfWarUnit)
    //    //{
    //    //    _fogOfWarUnit.circleRadius = 1f;
    //    //}
    //    //if (_damageable)
    //    //    _damageable.isStun = true;
    //}
    //public override void OnEnd()
    //{
    //    //base.OnEnd();
    //    //if (_target.photonView.ControllerActorNr == PhotonNetwork.LocalPlayer.ActorNumber)
    //    //{
    //    //    //_cameraCurseEventSO.RaiseEvent(false);
    //    //    BeautifySettings.settings.vignettingFade.value = .5f;

    //    //}
    //    //if(_fogOfWarUnit)
    //    //    _fogOfWarUnit.circleRadius = 3;
    //    //if (_damageable)
    //    //    _damageable.isStun = false;
    //}


}
