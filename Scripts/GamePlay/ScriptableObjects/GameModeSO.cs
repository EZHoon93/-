
using System.Linq;

using RotaryHeart.Lib.SerializableDictionary;

using UnityEngine;

public enum GameMode
{
    HideNSeek
}

[System.Serializable]
public class TeamInfoDic : SerializableDictionaryBase<GameMode , TeamInfo> { }

[System.Serializable]
public class TeamInfo
{
    public int seekerCount;
    public int hiderCount;
    public int maxPlayerCount => seekerCount + hiderCount;
}

[CreateAssetMenu(menuName = "GamePlay/GameMode")]
public class GameModeSO : DescriptionBaseSO
{
    [Header("Varaibles ")]
    [SerializeField] private GameMode _gameMode;
    [SerializeField] private TeamInfoDic _teamInfoDic;

    public GameMode CurrentMode => _gameMode;

   
    public void Setup(GameMode gameMode)
    {
        _gameMode = gameMode;
    }

    public byte GetMaxPlayers(GameMode gameMode)
    {
        return 8;
    }
}

