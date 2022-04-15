using System.Collections;
using System.Collections.Generic;


using TMPro;

using UnityEngine;
using UnityEngine.UI;



public struct ChacaterSkinObjects
{
    public GameObject charcter;
    public GameObject wepon;
    public GameObject accesoriess;

    public void SetActive(bool active)
    {
        charcter?.gameObject.SetActive(active);
        wepon?.gameObject.SetActive(active);
        accesoriess?.gameObject.SetActive(active);
    }
}

public class UICharacterView : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] UserInfoSO _userInfoSO;
    [SerializeField] SkinContainerSO _skinContainerSO;
    [Header("Transform")]
    [SerializeField] Transform _characterPanel;     //캐릭이 생성될 위치
    [SerializeField] Transform _weaponSkinPanel;    //무기가 생성될 위치.
    [SerializeField] Transform _accessoriessSkinPanel;    //무기가 생성될 위치.
    [Header("Button")]
    [SerializeField] Button _stateButton;
    [SerializeField] Button _leftButton;
    [SerializeField] Button _rightButton;
    [Header("Text")]
    [SerializeField] TextMeshProUGUI _avaterCountText;
    [SerializeField] TextMeshProUGUI _nickNameText;
    [SerializeField] TextMeshProUGUI _stateButtonText;



    List<ChacaterSkinObjects> _chacaterSkinsObjectList = new List<ChacaterSkinObjects>();
    int _currentIndex;

    private void Awake()
    {
        _leftButton.onClick.AddListener(OnClickLeftButton);
        _rightButton.onClick.AddListener(OnClickRightButton);
        _stateButton.onClick.AddListener(OnClickStateButton);
    }
    private void Start()
    {
        Init();
         
    }

    private void OnEnable()
    {
    
    }
    private void OnDisable()
    {
    }

    private void Init()
    {
        _chacaterSkinsObjectList.Clear();
        foreach (var hasAvaterData in _userInfoSO.hasAvaterList)
        {
            ChacaterSkinObjects chacaterSkinObjects;
            chacaterSkinObjects.charcter = CreateSkinObject(SkinType.Character, hasAvaterData.characterAvaterKey);
            chacaterSkinObjects.wepon= CreateSkinObject(SkinType.Weapon, hasAvaterData.weaponKey);
            chacaterSkinObjects.accesoriess = CreateSkinObject(SkinType.Accesoriess, hasAvaterData.accesoryKey);
            _chacaterSkinsObjectList.Add(chacaterSkinObjects);
        }
        _currentIndex = _currentIndex == -1 ? _userInfoSO.currentAvaterIndex : _currentIndex;
        ShowAvater();

        SetupUserNickName();
        UpdateCountText();
    }

    private void SetupUserNickName()
    {
        _nickNameText.text = _userInfoSO.nickName;
    }

    private void UpdateCountText()
    {
        _avaterCountText.text = $"{_currentIndex+1}/{_userInfoSO.hasAvaterList.Count}";
    }

    private void ShowAvater()
    {
        foreach (var ch in _chacaterSkinsObjectList)
            ch.SetActive(false);

        _chacaterSkinsObjectList[_currentIndex].SetActive(true);
        UpdateCountText();
        ShowCurrentState();
    }

    private void ShowCurrentState()
    {
        bool isUsing = _userInfoSO.currentAvaterIndex == _currentIndex ? true : false;
        _stateButton.interactable = !isUsing;
        var content = isUsing ? "사용 중" : "사용하기";
        _stateButtonText.text = content;
    }


    private void OnClickLeftButton()
    {
        _currentIndex = _currentIndex - 1 < 0 ?   _userInfoSO.hasAvaterList.Count -1: _currentIndex -1;
        ShowAvater();
    }
    private void OnClickRightButton()
    {
        _currentIndex = _currentIndex + 1 >= _userInfoSO.hasAvaterList.Count ? 0 : _currentIndex+1;
        ShowAvater();
    }
    private void OnClickStateButton()
    {
        _userInfoSO.currentAvaterIndex = _currentIndex;
        ShowCurrentState();
    }
    #region 오브젝 생성
    private GameObject CreateSkinObject(SkinType skinType, string prefabID)
    {
        var skinObject = _skinContainerSO.CreateSkinObject(skinType, prefabID);
        if (skinObject == null)
            return null;
        var parentTr = GetTransformParent(skinType);
        skinObject.transform.ResetTransform(parentTr);

        return skinObject;
    }

    private Transform GetTransformParent(SkinType skinType)
    {
        switch (skinType)
        {
            case SkinType.Accesoriess:
                return _accessoriessSkinPanel;
            case SkinType.Character:
                return _characterPanel;
            case SkinType.Weapon:
                return _weaponSkinPanel;
        }
        return null;
    }


    #endregion
}
