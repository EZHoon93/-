
using Photon.Pun;

using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);

         
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

  
        return  null;
    }

    public GameObject Instantiate(GameObject original, Transform parent = null)
    {
        return null;
    }


    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            return;
        }
        Object.Destroy(go);
    }

    /// <summary>
    /// PunPoolManagerBridge => Destory => Managers.Destroy
    /// </summary>
    /// <param name="go"></param>

    public void PunDestroy(MonoBehaviourPun go)
    {
        if (go.photonView.IsMine == false)
            return;

        PhotonNetwork.Destroy(go.gameObject);
    }
}
