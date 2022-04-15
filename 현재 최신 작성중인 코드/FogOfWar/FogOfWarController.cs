using FoW;

using Photon.Pun;

using UnityEngine;
public class FogOfWarController : MonoBehaviour
{
    [SerializeField] FogOfWarTeamRunTimeSet _fogOfWarTeamRunTimeSet;

    FogOfWarTeam _fogOfWarTeam;

    private void Awake()
    {
        _fogOfWarTeam = GetComponent<FogOfWarTeam>();
    }

    private void OnEnable()
    {
        _fogOfWarTeamRunTimeSet.Add(_fogOfWarTeam);
    }
    private void OnDisable()
    {
        _fogOfWarTeamRunTimeSet.Remove(_fogOfWarTeam);
    }






}
