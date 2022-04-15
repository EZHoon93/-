using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Scene Load EventChannel")]
public class SceneLoadEventChannelSO : DescriptionBaseSO
{
    public UnityAction<GameSceneSO> OnLoadingRequested;

    public void RaisedEvent(GameSceneSO loadGameScene )
    {
        OnLoadingRequested?.Invoke(loadGameScene);
    }
}
