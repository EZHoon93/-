﻿
using Photon.Pun;

public class GameState_End : GameState_Base

{
    public override float remainTime => 5;

    public override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        var winTeam = (Define.Team)info.photonView.InstantiationData[1];//승리
        var resultUI =  Managers.UI.ShowPopupUI<UI_GameResult>();
        var allPlayer = Managers.Game.GetAllPlayers();
        resultUI.Seup(winTeam, allPlayer);
        //string noticeContent = null;
        //if (winTeam == Define.Team.Hide)
        //{
        //    noticeContent = Util.GetColorContent(Color.blue, "숨는 팀승리!! ");
        //}
        //else
        //{
        //    noticeContent = Util.GetColorContent(Color.red, "술래 팀승리!! ");
        //}
        //uI_Main.UpdateNoticeText(noticeContent);
        //uI_Main.noticeBg.enabled = true;
        //uI_Main.titleText.text = "잠시 후 다른 게임으로 입장합니다!!";
    }
    public override void OnUpdate(int remainTime)
    {
        uI_Main.UpdateCountText(remainTime);
    }
    public override void OnTimeEnd()
    {
        Managers.PhotonManager.Clear();
        Managers.Scene.CallRandomGameScene();
        
    }

}
