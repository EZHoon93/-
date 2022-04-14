using System.Collections;
using System.Collections.Generic;

using EZPool;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class UIGameResult : UIPopBase
{

    [Header("Setup Variable")]
    [SerializeField] private PhotonGamingPlayersInfoSO _enterPlayersInfoSO;
    [SerializeField] private TransformPoolSO _gameResultITemPoolSO;
    [Header("Component")]
    [SerializeField] private Transform _winPanel;
    [SerializeField] private Transform _losePanel;
    [SerializeField] private TextMeshProUGUI _countDownText;

    [Header("Listening")]
    [SerializeField] private BoolEventChannelSO _isWhoTeamWinEventSO;
    [SerializeField] private GameStateTimerEventSO _gameStateTimerEventSO;




    private void OnEnable()
    {
        _gameStateTimerEventSO.onEventRaised += CallBackGameStateTimer;
    }
    private void OnDisable()
    {
        _gameStateTimerEventSO.onEventRaised -= CallBackGameStateTimer;
    }

    public void Setup(Define.Team winTeam)
    {
        var layOutGroup1 = _winPanel.GetComponent<VerticalLayoutGroup>();
        var layOutGroup2 = _losePanel.GetComponent<VerticalLayoutGroup>();
        layOutGroup1.enabled = false;
        layOutGroup2.enabled = false;

        var playerInfos = _enterPlayersInfoSO.GetPlayerGameInfos();
        foreach(var playerInfo in playerInfos)
        {
            var go = _gameResultITemPoolSO.Pop().GetComponent<UIResultInfo>();
            var panelTr = playerInfo.team == winTeam ? _winPanel : _losePanel;
            go.Setup(playerInfo);
            go.transform.ResetTransform(panelTr,true);
        }

        layOutGroup1.enabled = true;
        layOutGroup2.enabled = true;
        this.GetComponent<Canvas>().enabled = true;
    }

    void CallBackGameStateTimer(GameState gameState , int time)
    {
        if (gameState != GameState.End)
            return;

        _countDownText.text = time.ToString();
    }
}
