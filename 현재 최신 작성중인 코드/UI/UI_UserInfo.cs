
using Photon.Pun;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class UI_UserInfo : MonoBehaviour
{
    [SerializeField] private UserInfoSO _userInfoSO;
    [Header("Text")]
    [SerializeField] TextMeshProUGUI _nickNameText;
    [SerializeField] TextMeshProUGUI _coinText;
    [SerializeField] TextMeshProUGUI _gemText;
    [SerializeField] TextMeshProUGUI _levelText;
    [Header("Etc")]
    [SerializeField] Slider _expSlider;
    [SerializeField] Button _exitButton;
    [Header("BroadCasting")]
    [SerializeField] private CheckPopMessageEventCannelSO _popMessageEventCannelSO;
    [SerializeField] private VoidEventChannelSO _photonGameRoomExitEvent;

    [Header("Listening")]
    [SerializeField] private VoidEventChannelSO _onUpdateUserInfoEventSO;



    private void Awake()
    {
        _exitButton.onClick.AddListener(OnClickExit);
    }
  
    private void Start()
    {
        _onUpdateUserInfoEventSO.onEventRaised += UpdateUserInfo;
    }
    private void OnDestroy()
    {
        _onUpdateUserInfoEventSO.onEventRaised -= UpdateUserInfo;
    }

    private void OnEnable()
    {
        //임시 
        UpdateUserInfo();
    }

    public void LoadGamePlayScene(bool active)
    {
        _exitButton.gameObject.SetActive(active);
    }

    private void UpdateUserInfo()
    {
        Debug.Assert(_userInfoSO, "UserInfo Is Null.....");
        _nickNameText.text = _userInfoSO.nickName.ToString();
        _coinText.text = string.Format(_userInfoSO.coin.ToString() , "##.#");
        _gemText.text = string.Format(_userInfoSO.gem.ToString(), "##.#");
        _levelText.text = $"LV.{_userInfoSO.level.ToString()}";
        _expSlider.maxValue = _userInfoSO.maxExp;
        _expSlider.value = _userInfoSO.exp;
    }

    private void OnClickExit()
    {
        //_popMessageEventCannelSO.OnPopRaisedEvent("게임 종료 , ",  CallBackButtonConfirm );
    }


    private void CallBackButtonConfirm()
    {
        if (PhotonNetwork.CurrentRoom == null)
            Application.Quit();
        else
            _photonGameRoomExitEvent.RaiseEvent();

    }

}
