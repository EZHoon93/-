
using System.Collections.Generic;
using FoW;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// HideInFog 관리, FogOfWarManager에서 일괄 업데이트
/// </summary>
[RequireComponent(typeof( HideInFog))]
[RequireComponent(typeof(NavMeshObstacle))]
[RequireComponent(typeof(BoxCollider))]

public class HideInFogController : MonoBehaviour
{
    [SerializeField] FogTeamEventChannelSO _fogTeamEventChannelSO;
    [SerializeField] IntEventChannelSO _changeCameraViewEventSO;

    [SerializeField]
    List<HideInFog> _hideInFogs = new List<HideInFog>(4);
    public List<HideInFog> HideInFogs => _hideInFogs;
    private void Reset()
    {
        _hideInFogs = this.gameObject.GetComponentLists<HideInFog>();
    }
 
    private void OnEnable()
    {
        _fogTeamEventChannelSO.onEventRaised += UpdateHideInFog;
        _changeCameraViewEventSO.onEventRaised += CallBackChangeTeam;
    }

    private void OnDisable()
    {
        _fogTeamEventChannelSO.onEventRaised -= UpdateHideInFog;
        _changeCameraViewEventSO.onEventRaised -= CallBackChangeTeam;

    }


    public void Clear()
    {
        _hideInFogs.Clear();
    }
    public void AddHideInFog(HideInFog hideInFog)
    {
        if (_hideInFogs.Contains(hideInFog))
            return;
        _hideInFogs.Add(hideInFog);
    }
    public void RemoveHideInFog(HideInFog hideInFog)
    {
        if (!_hideInFogs.Contains(hideInFog))
            return;
        _hideInFogs.Remove(hideInFog);
    }
    public void UpdateHideInFog(FogOfWarTeam fow)
    {
        //foreach (var hideInFog in _hideInFogs)
            //hideInFog.UpdateInFog(fow, this.transform.position);
    }

    void SetActiveAllRenderer(bool active)
    {
        foreach (var hideInFog in _hideInFogs)
            hideInFog.SetActiveRender(active);
    }
    void SetActiveHideInFogs(bool active)
    {
        foreach (var hideInFog in _hideInFogs)
            hideInFog.enabled = active;
    }

    void CallBackChangeTeam(int team)
    {
        //foreach (var hideInFog in _hideInFogs)
        //    hideInFog.team = fogOfWar.TeamNumber;
        //CheckFogOfWar();    //카메라로 바라보는 오브젝트가 현재오브젝트랑 같은 것이라면, 
        //void CheckFogOfWar()
        //{
        //    if (_fogOfWar != null)
        //    {
        //        var isEquls = Equals(_fogOfWar, fogOfWar);
        //        SetActiveHideInFogs(!isEquls);  //같은 오브젝트라면 HideinFogs전부다 꺼줌
        //        SetActiveAllRenderer(isEquls);  //Hideinfogs전부 꺼주고 Render들은 전부켜준다.
        //    }
        //}
        
    }
    
}
