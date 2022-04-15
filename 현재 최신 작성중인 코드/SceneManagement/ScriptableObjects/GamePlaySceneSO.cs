using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlaySceneSO : GameSceneSO
{
    [SerializeField] private ItemSO[] _obtainItems;

    public ItemSO[] ObtainItems => _obtainItems;
}
