
using Photon.Pun;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;

using UnityEngine;
using Photon.Realtime;

[CreateAssetMenu(menuName = "Photon/Single/CurrentRoom PlayersInfo")]

public class CurrentRoomPlayersInfoSO : DescriptionBaseSO
{
    public bool IsGameJoinLocalPlayer()
    {
        if(PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("jn"))
        {
            return (bool)PhotonNetwork.LocalPlayer.CustomProperties["jn"];
        }
        else
        {
            return false;
        }
    }
    public int GetPlayersCount()
    {
        return PhotonNetwork.CurrentRoom.PlayerCount;
    }
    public Player[] GetGameReadyPlayers()
    {
        return PhotonNetwork.CurrentRoom.Players.Values.Where(s => (bool)s.CustomProperties["jn"] == true).ToArray();
    }

    public int GetGameReadyPlayersCount()
    {
        return GetGameReadyPlayers().Length;
    }
}
