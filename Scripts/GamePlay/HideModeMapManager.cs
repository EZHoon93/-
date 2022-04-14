using System;
using System.Collections;
using System.Linq;

using Photon.Pun;

using UnityEngine;

public class HideModeMapManager : MonoBehaviour
{
    [Header("Varaibles")]
    [SerializeField] private HideObjectCollections[] _hideObjectCollectionArrays;   //숨을수있는 오브젝트들 모음
    [SerializeField] private int _removeCount;  //초기 삭제할 오브젝트 수 


    [Header("Event Channel And Listening")]
    [SerializeField] private PhotonEventChannelSO _setupMapEventChannelSO;
    [Header("Listening")]
    [SerializeField] private BoolEventChannelSO _setActiveUILoading;


    private void Start()
    {
        //yield return new WaitForSeconds(2.0f);
        if (PhotonNetwork.IsMasterClient == false)
            return;
        
        InitMapDataToServer();
    }
 
    [ContextMenu("SetupByEditor")]
    public void SetupByEditor()
    {
        _hideObjectCollectionArrays = FindObjectsOfType<HideObjectCollections>();
        //foreach (var h in _hideObjectCollectionArrays)
        //    h.Setup();
        //var objectList = GameObject.FindGameObjectsWithTag("MapObject");
        //foreach(var o in objectList)
        //{
        //    var temp = new GameObject();
        //    temp.transform.position = o.transform.position;
        //    temp.transform.rotation = o.transform.rotation;
        //    temp.transform.localScale = Vector3.one;
        //    o.transform.SetParent(temp.transform);
        //}


    }

   
    [CallBack]
    public void OnCallBackMapData(object data  )
    {
        print("OnCallBack MapData ");
        //var indexArray = (int[])data;
        //int i = 0;
        //foreach (var objectIndex in indexArray)
        //{
        //    _hideObjectCollectionArrays[i].Setup(objectIndex);
        //    i++;
        //}
        _setActiveUILoading.RaiseEvent(false);
    }


    void InitMapDataToServer()
    {
        //var tempList = _hideObjectCollectionArrays.ToList();
        //_hideObjectCollectionArrays.OrderBy(g => Guid.NewGuid()).Take(_removeCount).ToList().ForEach(x => x.Remove());
        //_hideObjectCollectionArrays.Where(x => x.Index != -1).ToList().ForEach(x => x.SetupRandomIndex()); 
        //_setupMapEventChannelSO.RaiseEventToServer(_hideObjectCollectionArrays.Select(x => x.Index).ToArray());
        print("DATA send");
        _setupMapEventChannelSO.RaiseEventToServer(new object[] { 1});
    }


}
