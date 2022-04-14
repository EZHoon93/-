using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuManager : MonoBehaviour
{
    [SerializeField] Button _gameSerachButton;
    [SerializeField] GameObject[] _onlyLobbyObjects;    //로비전용 오브젝트 들 
    [SerializeField] GameObject _menuButton;
    [Header("Listening")]
    [SerializeField] private BoolEventChannelSO _localUserIsGaneJoinEventSO;    //로컬유저가 게임참가했는지 여부

    private void OnEnable()
    {
        _localUserIsGaneJoinEventSO.onEventRaised += OnLocalUserGameJoin;
    }
    private void OnDisable()
    {
        _localUserIsGaneJoinEventSO.onEventRaised -= OnLocalUserGameJoin;

    }
    public void OnLoadNoPhotonScene(GameSceneSO gameSceneSO)
    {
        _gameSerachButton.gameObject.SetActive(true);
    }

    public void OnLoadPhotonScene(GameSceneSO gameSceneSO)
    {
        _gameSerachButton.gameObject.SetActive(false);
    }
    private void OnLocalUserGameJoin(bool isjoin)
    {
        _menuButton.gameObject.SetActive(!isjoin);
    }

    void SetActiveLobbyObjects(bool active)
    {
        foreach (var obj in _onlyLobbyObjects)
            obj.gameObject.SetActive(active);
    }


}
