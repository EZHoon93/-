using System;
using System.Collections;
using System.Collections.Generic;

using Data;

using UnityEngine;


[Serializable]
public class SaveData
{
    public string _nickName;
    public int level;
    public int coin;
    public int gem;
    public float exp;

    public List<AvaterSlotInfo> avaterList = new List<AvaterSlotInfo>();

    //public string ToJson()
    //{
    //    return JsonUtility.ToJson(this);
    //}

    //public void LoadFromJson(string json)
    //{
    //    JsonUtility.FromJsonOverwrite(json, this);
    //}
}
