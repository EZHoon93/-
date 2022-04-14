using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInGameManager : MonoBehaviour
{
    #region Fields
    #endregion
 

    [SerializeField] private GameStateSO _gameStateSO;
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _seekerCountText;
    [SerializeField] private TextMeshProUGUI _hiderCountText;
    [Header("Button")]
    [SerializeField] private Button _goLobbyButton;
    [SerializeField] private Button _gameJoinButton;
    [SerializeField] private Button _gameExitButton;
    [Header("Listening")]
    [SerializeField] private BoolEventChannelSO _localUserIsGaneJoinEventSO;    //로컬유저가 게임참가했는지 여부
    [SerializeField] private IntEventChannelSO _hiderCountEventSO;
    [SerializeField] private IntEventChannelSO _seekerCountEventSO;
 
    #region Properties
    #endregion

    #region Life Cycle

    private void OnEnable()
    {
        _localUserIsGaneJoinEventSO.onEventRaised += OnGameJoin;
        _hiderCountEventSO.onEventRaised += OnChangeHiderCount;
        _seekerCountEventSO.onEventRaised += OnChangeSeekerCount;
        _seekerCountText.text = "0";
        _hiderCountText.text = "0";

    }
    private void OnDisable()
    {
        _localUserIsGaneJoinEventSO.onEventRaised -= OnGameJoin;
        _hiderCountEventSO.onEventRaised -= OnChangeHiderCount;
        _seekerCountEventSO.onEventRaised -= OnChangeSeekerCount;
    }

    private void Start()
    {
        OnGameJoin(false);
    }
    #endregion

    #region public
    #endregion


    #region private

    private void OnGameJoin(bool isJoin)
    {
        _gameJoinButton.gameObject.SetActive(!isJoin);
        _gameExitButton.gameObject.SetActive(isJoin);
        _goLobbyButton.gameObject.SetActive(!isJoin);
    }
 
    

    private void OnChangeHiderCount(int count)
    {
        _hiderCountText.text = count.ToString();
    }
    private void OnChangeSeekerCount(int count)
    {
        _seekerCountText.text = count.ToString();
    }
    #endregion

    #region Override, Interface
    #endregion
}
