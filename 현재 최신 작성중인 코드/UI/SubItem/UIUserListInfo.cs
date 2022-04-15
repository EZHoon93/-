using TMPro;
using UnityEngine;

public class UIUserListInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _levelText;
    [SerializeField] TextMeshProUGUI _nameText;

    public void Setup(int level, string nickName)
    {
        _levelText.text = level.ToString();
        _nameText.text = nickName;
    }
}
