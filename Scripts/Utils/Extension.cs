using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public static class Extension
{
	public static bool IsMaster()
    {
		return PhotonNetwork.IsMasterClient;
    }
	public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
	{
		return Util.GetOrAddComponent<T>(go);
	}


	public static int GetPlayerViewID(this MonoBehaviourPun go )
	{
		int viewID = go.photonView.ViewID;
		if(go.photonView.InstantiationData[1] != null)
        {
			if(go.photonView.InstantiationData.GetType() == typeof(Hashtable))
            {
				var HT = (Hashtable)go.photonView.InstantiationData[1];
				if (HT.ContainsKey("vid"))
                {
					viewID = (int)HT["vid"];
				}
			}
		}
		
		return viewID;
	}
	public static Transform MyFindChild(this Transform go , string name)
    {
		return  Util.FindChild(go.gameObject, name, true).transform;
    }
	public static T FindChild<T>(this GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
	{
		return Util.FindChild<T>(go, null, recursive);
    }
	
	public static List<T> GetComponentLists<T>(this GameObject go) where T : UnityEngine.Object
    {
		return go.GetComponentsInChildren<T>().ToList();
    }
	public static T[] GetComponentsChildrenRemoveMy<T>(this Transform go, bool recursive = false) where T : UnityEngine.Component
	{
		return Util.GetComponentsChildrenRemoveMy<T>(go, recursive);
	}

	public static bool IsValid(this GameObject go)
	{
		return go != null && go.activeSelf;
	}
	public static bool IsMyCharacter(this MonoBehaviourPun go)
	{
		if (IsValidAI(go.gameObject)) return false; //AI라면 False
		if (go.photonView.IsMine)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public static int ViewID(this MonoBehaviourPun go)
	{
		return go.photonView.ViewID;
	}

	public static bool IsValidAI(this GameObject go)
	{
		return go.CompareTag("AI");
	}

	public static void SetLayerRecursively(this GameObject go, int newLayer)
	{
		Util.SetLayerRecursively(go, newLayer);
	}

	public static bool CheckCreateTime(this MonoBehaviourPun go, float createTime)
    {
		return PhotonNetwork.Time <= createTime + 1 ? true : false;
	}

	public static void ResetTransform(this Transform go ,Transform parent = null, bool isScaleOne = false)
    {
        if (parent != null)
        {
			go.transform.SetParent(parent);
        }
		Util.UtillResetTransform(go );

        if (isScaleOne)
        {
			go.transform.localScale = Vector3.one;
        }
    }
}
