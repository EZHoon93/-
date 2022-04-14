using UnityEngine;
using UnityEngine.UI;

public class UIInGameMenu : MonoBehaviour
{
    [SerializeField] BoolVaraibleSO _localUserIsGameJoin;   //로컬유저 게임참가 여부
    [SerializeField] GameObject _gameJoinButton;
    [SerializeField] Button _gameExitButton;
    [SerializeField] GameObject _menuButtons;

    [SerializeField] private CheckPopMessageEventCannelSO _checkPopMessageEventCannelSO;
    [SerializeField] private BoolEventChannelSO _confirmGameExitEventSO;
    private void Awake()
    {
        _gameExitButton.onClick.AddListener(OnClickGameExit);
    }

    private void OnEnable()
    {
        print("OnEnable UIIngamemu"  );
        _localUserIsGameJoin.OnChangeValue += SetActive;
        SetActive();
    }
    private void OnDisable()
    {
        _localUserIsGameJoin.OnChangeValue -= SetActive;
    }

    public void SetActive()
    {
        var isJoin = _localUserIsGameJoin.Value;
        _gameJoinButton.gameObject.SetActive(!isJoin);
        _gameExitButton.gameObject.SetActive(isJoin);
        _menuButtons.gameObject.SetActive(!isJoin);
    }

    void OnClickGameExit()
    {
        _checkPopMessageEventCannelSO.OnPopRaisedEvent("t e s t ", () => _confirmGameExitEventSO.RaiseEvent(false));
    }
}
