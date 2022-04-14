using System.Collections;
using System.Collections.Generic;

using FoW;

using UnityEngine;
using UnityEngine.AI;

public class HideObjectCollections : MonoBehaviour
{
    [SerializeField] private Transform[] _mapObjects;

    private HideInFog _hideInFog;
    private NavMeshObstacle _navMeshObstacle;


    public int Index { get; private set; }

    private void Reset()
    {
        _mapObjects = this.transform.GetComponentsChildrenRemoveMy<Transform>();

    }
    private void Awake()
    {
        _hideInFog = GetComponent<HideInFog>();
    }

  
    public void Setup(int index)
    {
        Index = index;
        foreach (var m in _mapObjects)
            m.gameObject.SetActive(false);

        if (index < 0)
            return;
        var activeObejct = _mapObjects[index].gameObject;
        activeObejct.SetActive(true);

    }

    public void Remove()
    {
        Index = -1;
    }
    public void SetupRandomIndex()
    {
        Index = Random.Range(0, _mapObjects.Length);
    }
    
}
