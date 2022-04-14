using UnityEngine;


[CreateAssetMenu(menuName = "GameScene/Hide" , fileName = "HideScene_")]
public class HideSceneSO : GamePlaySceneSO
{
    [SerializeField] private GameObject[] _hidePrefabs;

    public GameObject[] HidePrefabs => _hidePrefabs;
}
