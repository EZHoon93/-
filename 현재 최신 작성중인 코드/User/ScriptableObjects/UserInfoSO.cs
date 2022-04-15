using System.Collections;
using System.Collections.Generic;

using Data;

using UnityEngine;

[CreateAssetMenu(menuName = "Single/UserInfo")]
public class UserInfoSO : DescriptionBaseSO
{
    private readonly string jsonDataName = "userData.json";
    public string key;
    public string nickName;
    public int level;
    public int coin;
    public int gem;
    public int exp;
    public int maxExp;


    public int currentAvaterIndex;

    public List<AvaterSlotInfo> hasAvaterList;



}
