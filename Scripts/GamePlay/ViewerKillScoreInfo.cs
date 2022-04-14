
using ExitGames.Client.Photon;

using UnityEngine;

public class ViewerKillScoreInfo : MonoBehaviour
{
    [Header("Virables")]
    [SerializeField] private PlayerRuntimeSet _playerRuntimeSet;    //살아있는 플레어들 
    [SerializeField] private PhotonGamingPlayersInfoSO _enterGamingPlayersInfoSO;
    [Header("BroadCasting And Listening")]
    [SerializeField] private NotifyKillInfoEventSO _notifyKillInfoEventSO;
    [SerializeField] private PhotonEventChannelSO _cacnhedPlayerScoreUpdateSO;


    //서버에서 죽은정보 바로 데이터저장을위해 캐쉬 서버로보냄 
    [CallBack]
    public void PhotonCallBackPlayerDie(object data)
    {
        //데이터 받아옴
        var datas = (object[])data;
        var killPlayerViewID = (int)datas[0];
        var deathPlayerViewID = (int)datas[1];
        //처리


        var killControllerNr = GetControllerNr(killPlayerViewID);
        var deathlControllerNr = GetControllerNr(deathPlayerViewID);

        //업데이트 스코어
        UpdatePlayerScoreToServer(killControllerNr, true);
        UpdatePlayerScoreToServer(deathlControllerNr, false);
        //UI이벤트
        NotifyKillInfo(killControllerNr, deathlControllerNr);


    }
    //캐쉬된 데이터   data 0 : kill컨트롤 ID , 1 death컨트롤 아이디
    [CallBack]
    public void PhotonCallBackPlayerScoreUpdate(object data)
    {
        var dataHT = (Hashtable)data;
        //---------------------------------------------------------------------------//
        var playerControllerNr = (int)dataHT["pn"];
        var killScore = (int)dataHT["kp"];
        var deathScore= (int)dataHT["dp"];
        _enterGamingPlayersInfoSO.UpdateScore(playerControllerNr, killScore, deathScore);

    }
    void NotifyKillInfo(int killControllerNr , int deathControllerNr)
    {
        var killPlayerNickName = _enterGamingPlayersInfoSO.GetPlayerNickName(killControllerNr);
        var deathPlayerNickName = _enterGamingPlayersInfoSO.GetPlayerNickName(deathControllerNr);
        _notifyKillInfoEventSO.RaiseEvent(killPlayerNickName, deathPlayerNickName); //UI호출
    }

    void UpdatePlayerScoreToServer(int playerControllerNr , bool addKillScore)
    {
        Hashtable dataHT = new Hashtable();
        var currentKillScore = _enterGamingPlayersInfoSO.GetPlayerKillScore(playerControllerNr);
        var deathScore = _enterGamingPlayersInfoSO.GetPlayerDeathScore(playerControllerNr);

        if (addKillScore)
            currentKillScore++;
        else
            deathScore++;

        dataHT.Add("pn", playerControllerNr);

        _cacnhedPlayerScoreUpdateSO.RaiseEventRemoveCached(dataHT); //키값만으로 일단제거

        dataHT.Add("kp", currentKillScore);
        dataHT.Add("dp", deathScore);

        _cacnhedPlayerScoreUpdateSO.RaiseEventToServer(dataHT);
    }


   

    int GetControllerNr(int viewID)
    {
        var playerController = _playerRuntimeSet.GetItem(viewID);
        if (playerController)
            return playerController.ControllerNr;

        return 0;
    }
}
