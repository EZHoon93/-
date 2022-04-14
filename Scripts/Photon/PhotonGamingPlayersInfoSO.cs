
using System;
using Photon.Pun;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine;
using Photon.Realtime;
using System.Collections.Generic;

public struct PlayerGameInfo
{
    public string nickName;
    public int killScore;
    public int deathScore ;
    public Define.Team team;
}

[CreateAssetMenu(menuName = "Photon/Single/Gameing PlayerInfo")]

public class PhotonGamingPlayersInfoSO : DescriptionBaseSO
{


    [Header("Virables")]
    [SerializeField] private PlayerRuntimeSet _playerRuntimeSet;    //살아있는 플레어들 
    [Header("BroadCasting")]
    [SerializeField] private PhotonEventChannelSO _photonEventChannelSO;

    private Hashtable _gamingPlayersInfoDataHT; //현재 게임에 참가중인 플레이어들 정보
    public Hashtable Data => _gamingPlayersInfoDataHT;


    public void Setup(object data)
    {
        _gamingPlayersInfoDataHT = (Hashtable)data;
    }


    public string GetPlayerNickName(int controllerNr)
    {
        var playerInfoHT = GetPlayerInfoHT(controllerNr);
        if (playerInfoHT == null)
            return null;

        if(playerInfoHT.TryGetValue("nn", out var result))
        {
            return (string)result;
        }

        return null;
    }



    public int GetPlayerKillScore(int controllerNr)
    {
        if(TryGetPlayerInfoHT(controllerNr, out var playerInfoHT))
        {
            if (playerInfoHT.ContainsKey("kp"))
            {
                return (int)playerInfoHT["kp"];
            }
            else
            {
                playerInfoHT.Add("kp", 0);
                return 0;
            }
        }
        return 0;
    }
    public int GetPlayerDeathScore(int controllerNr)
    {
        if (TryGetPlayerInfoHT(controllerNr, out var playerInfoHT))
        {
            if (playerInfoHT.ContainsKey("dp"))
            {
                return (int)playerInfoHT["dp"];
            }
            else
            {
                playerInfoHT.Add("dp", 0);
                return 0;
            }
        }
        return 0;
    }

    public void UpdateScore(int controllerNr, int killScore, int deathScore)
    {
        if(TryGetPlayerInfoHT(controllerNr,out var HT))
        {
            UpdateHT(HT, "kp", killScore);
            UpdateHT(HT,"dp", deathScore);
        }
    }
    public Define.Team GetPlayerTeam(int controllerNr)
    {
        if (TryGetPlayerInfoHT(controllerNr, out var playerInfoHT))
        {
            if (playerInfoHT.ContainsKey("te"))
            {
                return (Define.Team)playerInfoHT["te"];
            }
        }
        return default;
    }

    public PlayerGameInfo[] GetPlayerGameInfos()
    {
        List<PlayerGameInfo> result = new List<PlayerGameInfo>();
        foreach (var playerData in Data)
        {
            int playerControllerNr = (int)playerData.Key;
            //var playerInfoHT = GetPlayerInfoHT((int)playerData.Key);
            PlayerGameInfo playerGameInfo;

            playerGameInfo.nickName = GetPlayerNickName(playerControllerNr);
            playerGameInfo.killScore = GetPlayerKillScore(playerControllerNr);
            playerGameInfo.deathScore = GetPlayerDeathScore(playerControllerNr);
            playerGameInfo.team = GetPlayerTeam(playerControllerNr);
            result.Add(playerGameInfo);
        }

        return result.ToArray();
    }

    public bool IsLocalPlayerEnterGame()
    {
        var localControllerNr = PhotonNetwork.LocalPlayer.ActorNumber;
        var playerInfoHT = GetPlayerInfoHT(localControllerNr);
        return playerInfoHT != null ? true : false;
    }

    public Define.Team GetLocalPlayerTeam()
    {
        var localControllerNr = PhotonNetwork.LocalPlayer.ActorNumber;
        var playerInfoHT = GetPlayerInfoHT(localControllerNr);
        return GetPlayerTeam(localControllerNr);
    }

    //public 

    //int GetPlayerScore(int controllerNr)
    //{
    //    if (TryGetPlayerInfoHT(controllerNr, out var playerInfoHT))
    //    {
    //        if (playerInfoHT.ContainsKey("score"))
    //        {
    //            return (int)playerInfoHT["score"];
    //        }
    //    }
    //    return 0;
    //}
    //void AddScore(int controllerNr)
    //{
    //    if(TryGetPlayerInfoHT(controllerNr, out var playerInfoHT))
    //    {
    //        var score = (int)GetPlayerScore(controllerNr);
    //        playerInfoHT["score"] = score++;
    //        //_updatePlayerScoreEventSO.RaiseEventToServer(playerInfoHT);
    //        //PhotonNetwork.CurrentRoom.setc
    //    }
    //}

    void UpdateHT(Hashtable dataHT, object key , object value)
    {
        if (dataHT.ContainsKey(key))
        {
            dataHT[key] = value;
        }
        else
        {
            dataHT.Add(key, value);
        }
    }

    Hashtable GetPlayerInfoHT(int controllerNr)
    {
        if (_gamingPlayersInfoDataHT.TryGetValue(controllerNr, out object data))
        {
            if (data.GetType() == typeof(Hashtable))
            {
                return (Hashtable)data;
            }
        }
        return null;
    }
    bool TryGetPlayerInfoHT(int controllerNr , out Hashtable playerInfoHT)
    {
        playerInfoHT = null;
        var result = GetPlayerInfoHT(controllerNr);
        if (result == null)
            return false;

        playerInfoHT = result;
        return true;
    }
}
