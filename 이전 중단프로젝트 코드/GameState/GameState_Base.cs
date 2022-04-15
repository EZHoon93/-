using Photon.Pun;

public abstract class GameState_Base : MonoBehaviourPun 
{
    protected GameScene _gameScene;
    protected UI_Main uI_Main;
    protected bool isNextScene;
 
    protected virtual void OnEnable()
    {
        uI_Main = Managers.UI.SceneUI as UI_Main;
        _gameScene = Managers.Scene.currentScene as GameScene;
    }

    public abstract float remainTime { get; }
    public abstract void OnPhotonInstantiate(PhotonMessageInfo info);
    public abstract void OnUpdate(int remainTime);
    public abstract void OnTimeEnd();

    public virtual void OnPhotonDestroy()
    {

    }


    public void NextScene(Define.GameState gameState , object addData = null)
    {
        if (isNextScene) return;
        isNextScene = true;
        Managers.Spawn.GameStateSpawn(gameState, addData);

    }
}
