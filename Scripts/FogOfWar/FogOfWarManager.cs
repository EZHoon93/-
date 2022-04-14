
using System.Collections;
using UnityEngine;
using FoW;
using UnityEngine.Rendering;
using Photon.Pun;

public class FogOfWarManager : MonoBehaviour
{
    #region Varaibles
    [Header("Prefabs")]
    [SerializeField] private FogOfWarTeam _fogOfWarTeamPrefab;
    [SerializeField] private FogOfWarUnit _fogOfWarUnitPrefab;
    [Header("Varaible")]
    [SerializeField] private Volume _volume;
    [SerializeField] private PlayerRuntimeSet _playerRuntimeSet;
    [SerializeField] private FogOfWarTeamRunTimeSet _fogOfWarTeamRunTimeSet;
    [Header("Listening")]
    [SerializeField] private EachPhotonViewEventChannelSO _onPlayerInstantiateEventSO; //IPunInstantiateMagicCallback 콜백 시 발생
    [SerializeField] private EachPhotonViewEventChannelSO _onPlayerDestroyEventSO; //IPunInstantiateMagicCallback 콜백 시 발생
    [SerializeField] private IntEventChannelSO _onChangeCameraViewEvent;
    [Header("Broad Casting")]
    [SerializeField] private FogTeamEventChannelSO _fogTeamEventChannelSO;


    #endregion

    #region Life Cycle


    private void OnEnable()
    {
        _onPlayerInstantiateEventSO.onEventRaised += OnInstinatePlayerController;
        _onPlayerDestroyEventSO.onEventRaised += OnDestroyPlayerController;
        _onChangeCameraViewEvent.onEventRaised += OnChangeCameraViewID;
    }
    private void OnDisable()
    {
        _onPlayerInstantiateEventSO.onEventRaised -= OnInstinatePlayerController;
        _onPlayerDestroyEventSO.onEventRaised -= OnDestroyPlayerController;
        _onChangeCameraViewEvent.onEventRaised -= OnChangeCameraViewID;
    }

    #endregion

    #region Call Back Event


    private void OnChangeCameraViewID(int viewID)
    {
        var fogOfWarTeam = GetCurrentCameraViewFogOfWarTeam(viewID);
        foreach (var fog in _fogOfWarTeamRunTimeSet.Items)  //모두 끔
            fog.enabled = false;

        if (fogOfWarTeam == null)
            return;
        fogOfWarTeam.enabled = true;    //현재 보는것만 킴 

        StopAllCoroutines();
        StartCoroutine(UpdateCoroutine(fogOfWarTeam));

        VolumeProfile profile = _volume.sharedProfile;
        if (profile != null && profile.TryGet(out FogOfWarURP fow))
            fow.team.value = viewID;
    }
    private void OnInstinatePlayerController(PhotonView photonView)
    {
        var playerViewID = photonView.ViewID;
        if(_playerRuntimeSet.TryItem(playerViewID, out var playerController))
        {
            var fogOfWarTeam = Instantiate(_fogOfWarTeamPrefab, this.transform);
            var fogOfWarUnit = Instantiate(_fogOfWarUnitPrefab, playerController.transform);
            fogOfWarUnit.team = playerViewID;
            fogOfWarTeam.team = playerViewID;
        }
    }
    private void OnDestroyPlayerController(PhotonView photonView)
    {
        var playerViewID = photonView.ViewID;
        var fogOfWarTeam =  _fogOfWarTeamRunTimeSet.Items.Find(x => x.team == playerViewID);
        Destroy(fogOfWarTeam.gameObject);
    }
    #endregion

    #region private

    private FogOfWarTeam GetCurrentCameraViewFogOfWarTeam(int viewID)
    {
        var currentCameraViewTeam = _fogOfWarTeamRunTimeSet.Items.Find(x => x.team == viewID);
        if(currentCameraViewTeam == null)
        {
            return null;
        }
        return currentCameraViewTeam;
     
    }

    private IEnumerator UpdateCoroutine(FogOfWarTeam fogOfWarTeam)
    {
        var seconds = new WaitForSeconds(.1f);
        while (true)
        {
            if (fogOfWarTeam == null)
                break;
            fogOfWarTeam.ManualUpdate(1);
            _fogTeamEventChannelSO.RaiseEvent(fogOfWarTeam);
            yield return seconds;
        }
    }
    #endregion

}
