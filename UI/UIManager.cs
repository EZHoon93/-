using UnityEngine;
using UnityEngine.Events;

public class UIManager : GenricSingleton<UIManager>
{
    #region Fields
    [SerializeField] private UI_UserInfo _userInfo;
    [SerializeField] private UIInGameManager _uIInGameManager;
    [SerializeField] private UIPopUpManager _uIPopUpManager;
    [SerializeField] private UILoading _loadingUI;

    [Header("Listening")]
    [SerializeField] private BoolEventChannelSO _activeLoadingUIEvent;
    [SerializeField] private SceneLoadEventChannelSO _onLoadSceneEvent;   //씬호출 
    #endregion

    #region Life Cycle


    private void OnEnable()
    {
        _activeLoadingUIEvent.AddListener(SetActiveLoadingUI);
        _onLoadSceneEvent.OnLoadingRequested += OnLoadScene;
    }

    private void OnDisable()
    {
        _activeLoadingUIEvent.RemoveListener(SetActiveLoadingUI);
        _onLoadSceneEvent.OnLoadingRequested -= OnLoadScene;
    }

    #endregion
    #region public
    public GameObject OnPopUp(UIPopUpSO uIPopUpSO)
    {
        var go =  _uIPopUpManager.OnPopUp(uIPopUpSO);
        return go;
    }
    #endregion
    #region private



    private void SetActiveLoadingUI(bool active)
    {
        _loadingUI.gameObject.SetActive(active);
    }

    private void OnLoadScene(GameSceneSO gameSceneSO)
    {
        if(gameSceneSO.sceneType == GameSceneType.GamePlay)
        {
            _uIInGameManager.gameObject.SetActive(true);
        }
        else
        {
            _uIInGameManager.gameObject.SetActive(false);
        }
    }

   
    #endregion
}
