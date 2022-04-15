using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class InGameStore
{

}

public class DataManager
{
 //   public Define.GameDataState State { get; private set; }
   

 //   public void Init()
 //   {
 //       State = Define.GameDataState.Load;
 //   }

 //   Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
 //   {
 //       //TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
 //       //      //Debug.LogError(textAsset);
 //       //      return JsonUtility.FromJson<Loader>(textAsset.text);
 //       return default;
	//}
}
