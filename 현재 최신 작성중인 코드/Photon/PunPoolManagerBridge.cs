
using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class PunPoolManagerBridge : MonoBehaviour, IPunPrefabPool
{
    [SerializeField] GameObject[] _spawnList;
    private Dictionary<string, Stack<GameObject>> _poolDic = new Dictionary<string, Stack<GameObject>>();

    private void Start()
    {
        //PhotonNetwork.PrefabPool = this;
        //foreach (var spawn in _spawnList)
        //{
        //    var temp = new Stack<GameObject>();
        //    for (int i = 0; i < 8; i++)
        //    {
        //        temp.Push(Pop(spawn.name));
        //    }
        //    while (temp.Count > 0)
        //    {
        //        Push(temp.Pop());
        //    }
        //}
    }


    public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {
        var go = Pop(prefabId);
        go.transform.position = position;
        go.transform.rotation = rotation;
        go.transform.parent = null;
        return go;
    }
    public void Destroy(GameObject gameObject)
    {
        //"(Clone)" 7글자
        print("Destroy " + gameObject);
        var onPhotonViewPreNetDestroy = gameObject.GetComponents<IOnPhotonViewPreNetDestroy>();
        if (onPhotonViewPreNetDestroy != null)
        {
            foreach (var p in onPhotonViewPreNetDestroy)
            {
                p.OnPreNetDestroy(gameObject.GetPhotonView());
            }
        }
        Push(gameObject);

    }
    GameObject Pop(string prefabId)
    {
        Stack<GameObject> stack;
        if(_poolDic.TryGetValue(prefabId, out stack) == false)
        {
            stack = new Stack<GameObject>();
            _poolDic.Add(prefabId, stack);

#if UNITY_EDITOR
            var root = new GameObject(prefabId).transform;
            root.parent = this.transform;
#endif
        }

        if(stack.Count == 0)
        {
            return CreateGameObject(prefabId);
        }

        var go = stack.Pop();
        go.gameObject.SetActive(false);
        return go;
    }

    void Push(GameObject pushGameObject )
    {
        var prefabId = pushGameObject.name.Substring(0, pushGameObject.name.Length - 7);
        if (_poolDic.ContainsKey(prefabId))
        {
            pushGameObject.SetActive(false);
            _poolDic[prefabId].Push(pushGameObject);

#if UNITY_EDITOR
            var root = this.transform.Find(prefabId);
            if (root)
            {
                pushGameObject.transform.parent = root;
            }
#endif
        }
    }

    GameObject CreateGameObject(string prefabId)
    {
        var prefab = _spawnList.Single(x => string.Compare(x.name,prefabId) == 0);
        if (prefab == null)
        {
            Debug.LogError("에러 !!");
        }
        bool wasActive = prefab.activeSelf;
        //포톤의 생성을위해 false상태로 생성해줌
        if (wasActive)
            prefab.SetActive(false);

        var go = Instantiate(prefab);
        //다시 프리팹  복구
        if (wasActive)
            prefab.SetActive(true);

        return go;
    }

   


}