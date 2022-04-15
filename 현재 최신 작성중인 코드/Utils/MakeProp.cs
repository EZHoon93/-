using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeProp : MonoBehaviour
{
    [SerializeField] GameObject[] _gameObjects;
    //[SerializeField] GameObject _prop;

    [ContextMenu("Make")]
    public void Make()
    {
        foreach(var gameObj in _gameObjects)
        {
            var count = gameObj.transform.childCount;
            //var copyData = gameObj.
            List<GameObject> _copyData = new List<GameObject>();
            for(int i = 0; i < gameObj.transform.childCount; i++)
            {
                _copyData.Add(gameObj.transform.GetChild(i).gameObject);
            }

            foreach(var d in _copyData)
            {
                var parent = new GameObject("Prop");
                var childObject = d;
                childObject.transform.SetParent(parent.transform);

            }
            //for (int i = 0; i < count; i++)
            //{
            //        var parent = new GameObject("Prop");
            //    var childObject = gameObj.transform.GetChild(i);
            //        parent.transform.position = childObject.transform.position;
            //    childObject.transform.SetParent(parent.transform);
            //}
        }
    }

    [ContextMenu("Convert")]
    public void InverConvert()
    {
        foreach(var g in _gameObjects)
        {
            g.transform.position = g.transform.GetChild(0).transform.position;
            g.transform.GetChild(0).transform.localPosition = Vector3.zero;
        }
    }
}
