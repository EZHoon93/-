using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

/// <summary>
/// 캐릭터 오브젝트에 붙음. HideInFogController을 캐릭의LivingEntitiy HideInFogController에 값을 전달하고 본인것은 사용X
/// </summary>
public class Equipmentable : MonoBehaviour
{
    [SerializeField] Transform _model;
    [SerializeField] Define.SkinType _skinType;

    
    public Define.SkinType skinType => _skinType;
    public Transform model => _model;

    


    //public void OnPhotonInstantiate(PhotonMessageInfo info)
    //{
    //    var HT = (Hashtable)info.photonView.InstantiationData[1];
    //    var playerController = Managers.Game.GetPlayerController((int)HT["vid"]);
    //    Setup(playerController);
    //}
    public void Setup(PlayerController playerController)
    {
        if (playerController == null)
            return;
        //playerController.onPhotonDestroy += OnPhotonDestroy;
        //playerController.PlayerAvater.SetupEquipment(this);
        //_hideInFogController.enabled = false;
        //foreach (var hide in _hideInFogController.HideInFogs)
        //{
        //    playerController.HideInFogController.AddHideInFog(hide);
        //}
    }
    public void OnPhotonDestroy(PhotonView photonView)
    {

    }

  
}
