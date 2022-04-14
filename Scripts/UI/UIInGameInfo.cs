
using TMPro;
using UnityEngine;
using System.Linq;
public class UIInGameInfo : MonoBehaviour
{
    [Header("Varaibles")]
    [SerializeField] private GameStateSO _gameStateSO;
    [SerializeField] private DamageableRunTimeSet _damageableRunTime;
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _seekerCountText;
    [SerializeField] private TextMeshProUGUI _hiderCountText;
    [SerializeField] private TextMeshProUGUI _gameTimeText;
    [Header("Listening")]
    [SerializeField] private GameStateTimerEventSO _gameStateTimerEventSO;
    [SerializeField] private IntEventChannelSO _hiderCountEventSO;
    [SerializeField] private IntEventChannelSO _seekerCountEventSO;


    private void OnEnable()
    {
        _gameStateTimerEventSO.onEventRaised += UpdateTime;
        _hiderCountEventSO.onEventRaised += OnChangeHiderCount;
        _seekerCountEventSO.onEventRaised += OnChangeSeekerCount;
    }
    private void OnDisable()
    {
        _gameStateTimerEventSO.onEventRaised -= UpdateTime;
        _hiderCountEventSO.onEventRaised -= OnChangeHiderCount;
        _seekerCountEventSO.onEventRaised -= OnChangeSeekerCount;
    }
    private void Start()
    {
        UpdateTimeText(0);
        _seekerCountText.text = "0";
        _hiderCountText.text = "0";
    }

    void UpdateTime(GameState gameState, int time)
    {
        switch (gameState)
        {
            case GameState.Gameing:
                UpdateTimeText(time);
                break;
            default:
                break;
        }

    }

    private void OnChangeHiderCount(int count)
    {
        print("onChangeHider COunt");
        _hiderCountText.text = count.ToString();
    }
    private void OnChangeSeekerCount(int count)
    {
        _seekerCountText.text = count.ToString();
    }
 
    void UpdateTimeText(int time)
    {
        _gameTimeText.text = Util.GetTimeFormat(time);
    }

}
