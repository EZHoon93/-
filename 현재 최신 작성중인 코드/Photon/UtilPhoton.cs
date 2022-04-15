
using System.Linq;

using ExitGames.Client.Photon;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
public static class UtilPhoton 
{
    public static Hashtable GetPlayerHashData(PhotonView photonView)
    {
        var data = (Hashtable)photonView.InstantiationData[1];

        return data;
    }

    public static Hashtable GetDataHT(object data)
    {
        if(data == null || data.GetType() != typeof(Hashtable))
        {
            Debug.LogError("Error Not Hashtable Type");
            return null;
        }

        return (Hashtable)data;
    }

    public static int GetPlayerViewID(Hashtable dataHT)
    {
        //if(dataHT.TryGetValue(PhotonKeyConfig.playerViewID , out var value))
        //{
        //    if(value.GetType() == typeof(int))
        //        return (int)value;
        //}
        //Debug.LogError("Error Not GetPlayerViewID Type");
        //return 0;
        return GetTypeID<int>(dataHT, PhotonKeyConfig.playerViewID);
    }
    public static int GetPrefabID(Hashtable dataHT)
    {
        return GetTypeID<int>(dataHT, PhotonKeyConfig.prefabID);
    }

    public static T GetTypeID<T>(Hashtable dataHT , string key)
    {
        if (dataHT.TryGetValue(key, out var value))
        {
            if (value.GetType() == typeof(int))
                return (T)value;
        }
        Debug.LogError("Error Not  Type");
        return default;
    }

    public static int GetPlayerViewID(PhotonMessageInfo info)
    {
        int result = (int)info.photonView.InstantiationData[1];
        return result;
    }

    public static Define.Team GetPlayerTeam(PhotonView info)
    {
        var HT = GetPlayerHashData(info);
        if (HT.TryGetValue(PunHashKeyConfig.team, out var result))
        {

            return (Define.Team)result;
        }

        Debug.LogError("Not Find Player Team");
        return default;
    }

    public static bool IsGameJoinLocalPlayer()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("jn"))
        {
            return (bool)PhotonNetwork.LocalPlayer.CustomProperties["jn"];
        }
        else
        {
            return false;
        }
    }
    public static int GetPlayersCount()
    {
        return PhotonNetwork.CurrentRoom.PlayerCount;
    }
    public static Player[] GetGameReadyPlayers()
    {
        if(PhotonNetwork.IsConnected == false)
        {
            return null;
        }
        return PhotonNetwork.CurrentRoom.Players.Values.Where(s => (bool)s.CustomProperties["jn"] == true).ToArray();
    }

    public static int GetGameReadyPlayersCount()
    {
        return GetGameReadyPlayers().Length;
    }
}
