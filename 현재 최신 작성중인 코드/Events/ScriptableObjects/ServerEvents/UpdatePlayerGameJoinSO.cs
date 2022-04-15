
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;
public class UpdatePlayerGameJoinSO : DescriptionBaseSO
{
    //public UnityAction<Player Hashtable> onEventRaised;

    [SerializeField] IntVariableSO _enterjoinPlayerCount;
    [SerializeField] BoolVaraibleSO _isLocalPlayerIsGameJoin; //게임 참여 여부
    public void RaiseEvent(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("jn") == false)
            return;
        //_enterjoinPlayerCount.Value = PhotonHashConfig.GetEnterGameJoinUserCount();
        if (targetPlayer.IsLocal)
        {
            _isLocalPlayerIsGameJoin.Value = (bool)changedProps["jn"];
        }

    }
}

