
using TMPro;

using UnityEngine;

public class UIKillNotice : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textKillPlayer;
    [SerializeField] TextMeshProUGUI _deathKillPlayer;

    public void Setup(string killPlayer, string deathPlayer)
    {
        _textKillPlayer.text = killPlayer;
        _deathKillPlayer.text = deathPlayer;
    }
    
  
}
