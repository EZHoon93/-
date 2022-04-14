
using RotaryHeart.Lib.SerializableDictionary;

using UnityEngine;


[System.Serializable]
public class ControllerInfoDic : SerializableDictionaryBase<string, UIControllerInfo>{ }

[CreateAssetMenu(menuName = "Single/SettingInfo")]
public class SettingInfoSO : DescriptionBaseSO
{
    public float bgmValue;
    public float sfxValue;
    public bool _isRightHand;
    public ControllerInfoDic controllerInfoDic;


}
