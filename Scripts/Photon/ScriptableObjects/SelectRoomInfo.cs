using UnityEngine;

public class SelectRoomInfo : DescriptionBaseSO
{
    public string roomName;
    public bool isSecret;
    public GameMode gameMode;
    private void OnEnable()
    {
        roomName = null;
    }

}
