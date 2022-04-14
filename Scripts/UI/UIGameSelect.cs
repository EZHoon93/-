using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class UIGameSelect : UIPopBase
{
    [SerializeField] private GameObject _contentPanel;
    [SerializeField] private Toggle _itemToggle;
    [SerializeField] private Toggle _scretToggle;
    [SerializeField] private TMP_InputField _roomInput;
    [SerializeField] private SelectRoomInfo _selectRoomInfo;

    [Header("BroadCasting")]
    [SerializeField] private VoidEventChannelSO _searchGameRoomEventSO;
    [Header("Listening")]
    [SerializeField] private SceneLoadEventChannelSO _sceneLoadEventChannelSO;

  
    public void OnButtonClick_Confirm()
    {
        _selectRoomInfo.roomName = _roomInput.text.ToString();
        _selectRoomInfo.isSecret = _scretToggle.isOn;
        _selectRoomInfo.gameMode = GameMode.HideNSeek;
        this.gameObject.SetActive(false);
        _searchGameRoomEventSO.RaiseEvent();
    }
   
}
