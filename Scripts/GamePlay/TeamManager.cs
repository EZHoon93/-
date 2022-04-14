
using System;
using System.Linq;

using Photon.Pun;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private PhotonGamingPlayersInfoSO _photonGamingPlayersInfoSO;  //게임에 참가성공후 게임플레이어할 유저들정보
    [Header("Listening")]
    [Header("BroadCasting")]
    [SerializeField] private PhotonEventChannelSO _setupPlayerTeamChannelSO;    //팀셋업 서버로 전송
    //[SerializeField] private VoidEventChannelSO _teamSetupCallEventSO;  //팀 셋업 호출
    [SerializeField] private VoidEventChannelSO _onLoadTeamInfo;  //팀 셋업 호출




    /// <summary>
    /// 포톤 서버로부터 팀 셋업 콜백
    /// </summary>
    public void CallBackTeamInfo(object data)
    {
        _photonGamingPlayersInfoSO.Setup(data);
        _onLoadTeamInfo.RaiseEvent();
    }
    /// <summary>
    /// 포톤으로 팀 셋업 전송
    /// </summary>
    public void CallSetupTeamInfoToServer()
    {
        if (PhotonNetwork.IsMasterClient == false)
            return;
        var infoHT = new Hashtable();
        GetJoinUserData(infoHT);
        AddAISkinnfo(infoHT);
        //SelectUserSeeker(infoHT);
        _setupPlayerTeamChannelSO.RaiseEventToServer(infoHT);
    }

    #region Private Function




    void GetJoinUserData(Hashtable infoHT)
    {
        foreach (var player in PhotonNetwork.CurrentRoom.Players.Values.Where(s => (bool)s.CustomProperties["jn"] == true).ToList())
        {
            var HT = player.CustomProperties;
            infoHT.Add(player.ActorNumber, new Hashtable
            {
                ["nn"] = "ddddddddEZ",          //닉네임
                ["te"] = Define.Team.Seek,           //팀
                ["ch"] = 0,                          //캐릭스킨
                ["we"] = 0,                        //무기스킨
                ["ac"] = 0,                        //악세스킨
            });
        }
    }

    /// <summary>
    /// AI 추가 
    /// </summary>
    void AddAISkinnfo(Hashtable infoHT)
    {
        for (int i = infoHT.Count; i < 2; i++)
        {
            infoHT.Add(-i, new Hashtable
            {
                //["nu"] = -i,        //넘버
                ["nn"] = "AIPlayer2",          //닉네임
                ["te"] = Define.Team.Hide,           //팀
                ["ch"] = 0,                          //캐릭스킨
                ["we"] = 0,                        //무기스킨
                ["ac"] = 0,                        //악세스킨
            });
        }
    }

    void SelectUserSeeker(Hashtable infoHT)
    {
        var totSeekerCount = 2;
        var joinUserCount = infoHT.Keys.Count(p => (int)p > 0);
        var seletUserSeekerCount = GetUserSeekerCount(joinUserCount);
        var selectAISeekerCount = totSeekerCount - seletUserSeekerCount;

        //싱글모드일떄 (참여한 유저가 혼자)=> 전체 랜덤진행
        if (joinUserCount == 1)
        {
            infoHT.OrderBy(g => Guid.NewGuid()).Take(totSeekerCount).Select(a => a.Value).ToList().ForEach(v => ChangeTeamToSeek(v));
        }
        //참여한 유저가 둘이상/
        else
        {
            var userSeekerDic = infoHT.Keys.Where(p => (int)p >= 0).OrderBy(g => Guid.NewGuid()).Take(0);
            infoHT.Where(p => (int)p.Key >= 0).OrderBy(g => Guid.NewGuid()).Take(seletUserSeekerCount).ToList().ForEach(x => ChangeTeamToSeek(x.Value));
            infoHT.Where(p => (int)p.Key < 0).OrderBy(g => Guid.NewGuid()).Take(selectAISeekerCount).ToList().ForEach(x => ChangeTeamToSeek(x.Value));
        }
    }

    void ChangeTeamToSeek(object data)
    {
        var HT = (Hashtable)data;
        HT["te"] = Define.Team.Hide;
    }

    int GetUserSeekerCount(int userCount)
    {
        if (userCount == 1)
        {
            return 0;
        }
        else if (userCount >= 2 && userCount < 5)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }
    #endregion
}
