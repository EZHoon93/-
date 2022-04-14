
using System;
using System.Linq;

using Photon.Pun;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private PhotonGamingPlayersInfoSO _photonGamingPlayersInfoSO;  //���ӿ� ���������� �����÷��̾��� ����������
    [Header("Listening")]
    [Header("BroadCasting")]
    [SerializeField] private PhotonEventChannelSO _setupPlayerTeamChannelSO;    //���¾� ������ ����
    //[SerializeField] private VoidEventChannelSO _teamSetupCallEventSO;  //�� �¾� ȣ��
    [SerializeField] private VoidEventChannelSO _onLoadTeamInfo;  //�� �¾� ȣ��




    /// <summary>
    /// ���� �����κ��� �� �¾� �ݹ�
    /// </summary>
    public void CallBackTeamInfo(object data)
    {
        _photonGamingPlayersInfoSO.Setup(data);
        _onLoadTeamInfo.RaiseEvent();
    }
    /// <summary>
    /// �������� �� �¾� ����
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
                ["nn"] = "ddddddddEZ",          //�г���
                ["te"] = Define.Team.Seek,           //��
                ["ch"] = 0,                          //ĳ����Ų
                ["we"] = 0,                        //���⽺Ų
                ["ac"] = 0,                        //�Ǽ���Ų
            });
        }
    }

    /// <summary>
    /// AI �߰� 
    /// </summary>
    void AddAISkinnfo(Hashtable infoHT)
    {
        for (int i = infoHT.Count; i < 2; i++)
        {
            infoHT.Add(-i, new Hashtable
            {
                //["nu"] = -i,        //�ѹ�
                ["nn"] = "AIPlayer2",          //�г���
                ["te"] = Define.Team.Hide,           //��
                ["ch"] = 0,                          //ĳ����Ų
                ["we"] = 0,                        //���⽺Ų
                ["ac"] = 0,                        //�Ǽ���Ų
            });
        }
    }

    void SelectUserSeeker(Hashtable infoHT)
    {
        var totSeekerCount = 2;
        var joinUserCount = infoHT.Keys.Count(p => (int)p > 0);
        var seletUserSeekerCount = GetUserSeekerCount(joinUserCount);
        var selectAISeekerCount = totSeekerCount - seletUserSeekerCount;

        //�̱۸���ϋ� (������ ������ ȥ��)=> ��ü ��������
        if (joinUserCount == 1)
        {
            infoHT.OrderBy(g => Guid.NewGuid()).Take(totSeekerCount).Select(a => a.Value).ToList().ForEach(v => ChangeTeamToSeek(v));
        }
        //������ ������ ���̻�/
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
