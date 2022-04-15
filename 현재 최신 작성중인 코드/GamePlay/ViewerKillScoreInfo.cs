
using ExitGames.Client.Photon;

using UnityEngine;

public class ViewerKillScoreInfo : MonoBehaviour
{
    [Header("Virables")]
    [SerializeField] private PlayerRuntimeSet _playerRuntimeSet;    //����ִ� �÷���� 
    [SerializeField] private PhotonGamingPlayersInfoSO _enterGamingPlayersInfoSO;
    [Header("BroadCasting And Listening")]
    [SerializeField] private NotifyKillInfoEventSO _notifyKillInfoEventSO;
    [SerializeField] private PhotonEventChannelSO _cacnhedPlayerScoreUpdateSO;


    //�������� �������� �ٷ� ���������������� ĳ�� �����κ��� 
    [CallBack]
    public void PhotonCallBackPlayerDie(object data)
    {
        //������ �޾ƿ�
        var datas = (object[])data;
        var killPlayerViewID = (int)datas[0];
        var deathPlayerViewID = (int)datas[1];
        //ó��


        var killControllerNr = GetControllerNr(killPlayerViewID);
        var deathlControllerNr = GetControllerNr(deathPlayerViewID);

        //������Ʈ ���ھ�
        UpdatePlayerScoreToServer(killControllerNr, true);
        UpdatePlayerScoreToServer(deathlControllerNr, false);
        //UI�̺�Ʈ
        NotifyKillInfo(killControllerNr, deathlControllerNr);


    }
    //ĳ���� ������   data 0 : kill��Ʈ�� ID , 1 death��Ʈ�� ���̵�
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
        _notifyKillInfoEventSO.RaiseEvent(killPlayerNickName, deathPlayerNickName); //UIȣ��
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

        _cacnhedPlayerScoreUpdateSO.RaiseEventRemoveCached(dataHT); //Ű�������� �ϴ�����

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
