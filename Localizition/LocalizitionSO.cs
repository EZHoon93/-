using System.Collections;
using System.Collections.Generic;

using RotaryHeart.Lib.SerializableDictionary;

using UnityEngine;

public enum Lanugage
{
    Kor,
    Eng
}

[System.Serializable]
public class LaungageDic : SerializableDictionaryBase<Lanugage , string> {  };

[CreateAssetMenu(menuName = "Localiztion" , fileName = "Loc_")]
public class LocalizitionSO : DescriptionBaseSO
{
    [SerializeField] string _content;
    [SerializeField] LaungageDic _languageDic;

    public string GetContent(Lanugage lanugage)
    {
        if (_languageDic.ContainsKey(lanugage))
        {
            return _languageDic[lanugage];
        }
        return null;
    }
}
