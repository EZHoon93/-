using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System;

[System.Serializable]
public class PlayerList
{
    public List<PlayerController> playerControllerList = new List<PlayerController>();
}

[System.Serializable]
public class TeamPlayerDictionary : SerializableDictionaryBase<Define.Team, PlayerList> { };

[CreateAssetMenu(menuName = "RunTime/TeamRunTime", fileName = "RunTime_")]

public class TeamRunTimeSet : DescriptionBaseSO
{
    [SerializeField] private TeamPlayerDictionary _temaDic = new TeamPlayerDictionary();
    

    public int SeekerCount => _temaDic[Define.Team.Seek].playerControllerList.Count;
    public int HiderCount => _temaDic[Define.Team.Hide].playerControllerList.Count;


    public event Action onChangeEvent;
 

    public virtual void Add(PlayerController player)
    {
        PlayerList playerList;
        //if(_temaDic.TryGetValue(player.Team , out playerList ) == false)
        //{
        //    playerList = new PlayerList();
        //    _temaDic.Add(player.Team, playerList);
        //}
        //if(playerList.playerControllerList.Contains(player) == false)
        //{
        //    playerList.playerControllerList.Add(player);
        //    onChangeEvent?.Invoke();
        //}
    }

    public virtual void Remove(PlayerController player)
    {

        //if (_temaDic.ContainsKey(player.Team))
        //{
        //    _temaDic[player.Team].playerControllerList.Remove(player);
        //    onChangeEvent?.Invoke();

        //}
    }
}
