using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using Data;
using UnityEngine.UI;

public class UIResultInfo : MonoBehaviour
{
    [SerializeField] 
    Image[] _images;
    [SerializeField]
    TextMeshProUGUI _levelText;
    [SerializeField]
    TextMeshProUGUI _nickNameText;
    [SerializeField]
    TextMeshProUGUI _killText;
    [SerializeField]
    TextMeshProUGUI _deathText;


    public void Setup(PlayerGameInfo playerGameInfo)
    {
        _nickNameText.text = playerGameInfo.nickName;
        _killText.text = playerGameInfo.killScore.ToString();
        _deathText.text = playerGameInfo.deathScore.ToString();
    }
}
