using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopSetting : UIPopBase
{
    [SerializeField] SettingInfoSO _settingInfoSO;
    [SerializeField] Slider _bgmSlider;
    [SerializeField] Slider _sfxSlider;


    private void OnEnable()
    {
        _bgmSlider.value      = _settingInfoSO.bgmValue;
        _sfxSlider.value        = _settingInfoSO.sfxValue;

        _bgmSlider.onValueChanged.AddListener(OnChangeBgmValue);
        _sfxSlider.onValueChanged.AddListener(OnChangeSfxValue);
    }
    private void OnDisable()
    {
        _bgmSlider.onValueChanged.RemoveListener(OnChangeBgmValue);
        _sfxSlider.onValueChanged.RemoveListener(OnChangeSfxValue);
    }

    private void OnChangeBgmValue(float value)
    {
        _settingInfoSO.bgmValue = value;
    }

    private void OnChangeSfxValue(float value)
    {
        _settingInfoSO.sfxValue= value;
    }



}
