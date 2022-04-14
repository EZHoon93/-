



using UnityEngine;
using UnityEngine.AddressableAssets;

public enum GameSceneType
{
    login,
    Lobby,
    Loading,
    GamePlay
}

[CreateAssetMenu(menuName = "GameScene/GameScene")]
public class GameSceneSO : DescriptionBaseSO
{
    public GameSceneType sceneType;
    public string gameSceneName;
    public AssetReference assetReference;



}
