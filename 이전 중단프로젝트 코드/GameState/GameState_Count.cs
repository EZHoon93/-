
using System.Linq;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameState_Count : GameState_Base
{
    public override float remainTime => 10;
    public override void OnPhotonInstantiate(PhotonMessageInfo info )
    {
        uI_Main = Managers.UI.SceneUI as UI_Main;
        uI_Main.UpdateNoticeText("잠시 후 게임이 시작됩니다.");
    }
    public override void OnUpdate(int remainTime)
    {
        uI_Main.UpdateCountText(remainTime);
        Managers.Sound.Play("TimeCount", Define.Sound.Effect);
        CheckAnyReadyJoinUser();
    }
  
    public override void OnTimeEnd()
    {
        //방장만 실행.. 캐릭터 생성.
        if (PhotonNetwork.IsMasterClient)
        {
            var HT = GetPlayerSpawnData();
            NextScene(Define.GameState.GameReady, HT);   //다음 게임 단계로 진행 .. 
        }
    }


    /// <summary>
    /// 1명이라도 참가한유저가있다면 true,
    /// </summary>
    void CheckAnyReadyJoinUser()
    {
        if (PhotonNetwork.IsMasterClient == false) return;

        bool isExistUser = PhotonNetwork.CurrentRoom.Players.Values.Any(s => (bool)s.CustomProperties["jn"] == true); ;
        if (isExistUser == false)
        {
            Managers.Spawn.GameStateSpawn(Define.GameState.Wait);
        }
    }

    #region Make Player Hide&Seek Data
    public Hashtable GetPlayerSpawnData()
    {
        var HT = GetJoinUserData(); //참여한 유저 갖고옴
        HT = AddAISkinnfo(HT);  //나머지 최대정원까지 AI추가
        HT = SelectSeeker(HT);  //AI 선정.
        return HT;
    }

    Hashtable SelectSeeker(Hashtable HT)
    {
        //var seeketNumDic = HT.Keys.OrderBy(g => Guid.NewGuid()).Take(Managers.Scene.currentGameScene.totSeekerCount);
        var seeketNumDic = HT.Keys.OrderBy(g => (int)g == PhotonNetwork.LocalPlayer.ActorNumber).Take(1);
        
        foreach (var seekerNum in seeketNumDic)
        {
            if (HT.ContainsKey(seekerNum))
            {
                var changeHT = (Hashtable)HT[seekerNum];
                changeHT["te"] = Define.Team.Seek;
            }
        }

        var ss =(Hashtable) HT[PhotonNetwork.LocalPlayer.ActorNumber];
        ss["te"] = Define.Team.Hide;


        return HT;
    }
    Hashtable GetJoinUserData()
    {
        Hashtable result = new Hashtable();
        foreach (var player in PhotonNetwork.CurrentRoom.Players.Values.Where(s => (bool)s.CustomProperties["jn"] == true).ToList())
        {
            var HT = player.CustomProperties;
            result.Add(player.ActorNumber, new Hashtable
            {
                ["nn"] = player.NickName,          //닉네임
                ["te"] = Define.Team.Hide,           //팀
                ["ch"] = 0,                          //캐릭스킨
                ["we"] = 0,                        //무기스킨
                ["ac"] =0,                        //악세스킨
            });
        }
        return result;
    }
    Hashtable AddAISkinnfo(Hashtable HT)
    {
        for (int i = HT.Count; i < Managers.Scene.currentGameScene.maxPlayerCount; i++)
        {
            HT.Add(-i, new Hashtable
            {
                //["nu"] = -i,        //넘버
                ["nn"] = Managers.aIManager.GetRandomNickName(),          //닉네임
                ["te"] = Define.Team.Hide,           //팀
                ["ch"] = 0,                          //캐릭스킨
                ["we"] = 0,                        //무기스킨
                ["ac"] = 0,                        //악세스킨
            });
        }

        return HT;
    }

    #endregion


}
