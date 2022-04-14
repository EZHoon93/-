
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

using UnityEngine;
using System.Linq;

public class PhotonHashConfig : DescriptionBaseSO
{
    //public static bool LocalPlayerIsGameJoin
    //{
    //    get => (bool)PhotonNetwork.LocalPlayer.CustomProperties["jn"];
    //    set
    //    {
    //        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "jn", value } } );
    //    }
    //}

    //public static int SetLocalPlayerLevel(int level)
    //{
    //    PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "lv", level } });
    //    return 0;
    //}


    //public static int GetEnterGameJoinUserCount()
    //{
    //    return PhotonNetwork.CurrentRoom.Players.Values.Count(s => (bool)s.CustomProperties["jn"] == true); 
    //}

    //public static string GetPlayerNickName(object dataHT)
    //{
    //    var HT = (Hashtable)dataHT;
    //    if(HT.TryGetValue("nn", out var nickName))
    //    {
    //        return (string)nickName;
    //    }
    //    return null;
    //}

    //public static string GetPlayerNickName(Hashtable dataHT, int controllerNr)
    //{
    //    if (dataHT.TryGetValue("nn", out var nickName))
    //    {
    //        return (string)nickName;
    //    }
    //    return null;
    //}
}
