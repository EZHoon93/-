using TMPro;
using UnityEngine;

public class UICountDown : MonoBehaviour
{
    [SerializeField] PhotonGamingPlayersInfoSO _enterGamingPlayers;
    [Header("Text")]
    [SerializeField] TextMeshProUGUI _countDownText;
    [SerializeField] TextMeshProUGUI _inGameTimeText;

    //[SerializeField] TextMeshProUGUI _teamText;
    [SerializeField] GameObject _hideTeamObject;
    [SerializeField] GameObject _seekTeamObject;


    [Header("Listening")]
    [SerializeField] private GameStateTimerEventSO _gameStateTimerEventSO;
    [SerializeField] private VoidEventChannelSO _waitStateEventSO;
    [SerializeField] private VoidEventChannelSO _gameReadyEventSO;
    [SerializeField] private VoidEventChannelSO _gameingEventSO;


    private void OnEnable()
    {
        _gameStateTimerEventSO.onEventRaised += UpdateTime;
        _waitStateEventSO.onEventRaised += OnWaitState;
        _gameReadyEventSO.onEventRaised += OnGameReadyState;
        _gameingEventSO.onEventRaised += OnGameingState;

        Resets();
    }
    private void OnDisable()
    {
        _gameStateTimerEventSO.onEventRaised -= UpdateTime;
        _waitStateEventSO.onEventRaised -= OnWaitState;
        _gameReadyEventSO.onEventRaised -= OnGameReadyState;
        _gameingEventSO.onEventRaised -= OnGameingState;
    }
    private void Start()
    {
 
    }
    private void OnDestroy()
    {
      

    }
    private void Resets()
    {
        _countDownText.text = null;
        _countDownText.enabled = false;
        _hideTeamObject.gameObject.SetActive(false);
        _seekTeamObject.gameObject.SetActive(false);
    }


    private void UpdateTime(GameState gameState , int time)
    {
        switch (gameState)
        {
            case GameState.CountDown:
                _countDownText.text = time.ToString();
                _countDownText.enabled = true;
                break;
            case GameState.GameReady:
                _countDownText.text = time.ToString();
                _countDownText.enabled = true;
                break;
            case GameState.Gameing:
                _inGameTimeText.text = Util.GetTimeFormat(time);

                break;
            default:
                _countDownText.enabled = false;
                break;
        }
    }

    private void OnWaitState()
    {
        Resets();
    }

    private void OnGameReadyState()
    {
        var IsEnterGame = _enterGamingPlayers.IsLocalPlayerEnterGame();
        if (IsEnterGame)
        {
            var localPlayerTeam = _enterGamingPlayers.GetLocalPlayerTeam();
            switch (localPlayerTeam)
            {
                case Define.Team.Hide:
                    _hideTeamObject.gameObject.SetActive(true);
                    break;
                case Define.Team.Seek:
                    _seekTeamObject.gameObject.SetActive(true);
                    break;
            }
        }

    }

    private void OnGameingState()
    {
        Resets();
    }




}
