
using UnityEngine;


[CreateAssetMenu(menuName =  "Container/Scene")]
public class GameSceneContainerSO : DescriptionBaseSO
{
    [SerializeField]
    private HideSceneSO[] _hideScenesSO;


    public HideSceneSO[] HideScenesSO => _hideScenesSO;








    public GameSceneSO GetRandomGameScene(GameMode gameMode , string currentSceneName)
    {
        var gameSceneList = GetScene(gameMode);
        GameSceneSO selectGameSceneSO = null;
        byte ranMapCode;
        currentSceneName = null;    //테스트로 null;
        do
        {
            ranMapCode = (byte)Random.Range(0, gameSceneList.Length);
            selectGameSceneSO = gameSceneList[ranMapCode];
        } while (string.Equals(selectGameSceneSO.gameSceneName, currentSceneName));
        selectGameSceneSO = gameSceneList[ranMapCode];
        return selectGameSceneSO;
    }

    public GameSceneSO GetGameSceneSO(GameMode gameMode, string sceneName)
    {
        var gameSceneSOs = GetScene(gameMode);
        foreach(var gameSceneSO in gameSceneSOs)
        {
            if( string.Equals(gameSceneSO.gameSceneName, sceneName))
            {
                return gameSceneSO;
            }
        }
        return null;
        
    }
    private GameSceneSO GetSceneInfo(GameMode gameMode, byte mapCode)
    {
        var gaemScens = GetScene(gameMode);
        return gaemScens[mapCode];
    }
    private GameSceneSO[] GetScene(GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.HideNSeek:
                return _hideScenesSO;
        }
        return null;
    }

}
