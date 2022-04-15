using System.Collections;
using System.Collections.Generic;

using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using UnityEngine.UI;

public class UI_UserList : MonoBehaviour
{
    [SerializeField] Transform _content;
    [SerializeField] Transform _itemPanel;

    Dictionary<int, UIUserListInfo> _playerDic = new Dictionary<int, UIUserListInfo>();

    public UIUserListInfo userInfoPrefab;

    void Start()
    {
        _content.gameObject.SetActive(false);
        LoadCurrentPlayer();
        //Managers.photonGameManager.AddEventListenr(Define.InGamePhotonEvent.Enter, UpdatePlayer);
        //Managers.photonGameManager.AddEventListenr(Define.InGamePhotonEvent.Left, LeftrPlayer);
    }

    void LoadCurrentPlayer()
    {
        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            UpdatePlayer(player.Value);
        }
    }

  
    public void UpdatePlayer(Player newPlayer)
    {
        UIUserListInfo go = null;
        bool isExist = _playerDic.TryGetValue(newPlayer.ActorNumber, out go);
        if(isExist == false)
        {
            go = Instantiate(userInfoPrefab);
            go.transform.ResetTransform(_itemPanel);
            _playerDic.Add(newPlayer.ActorNumber, go);
        }
        var userLevel = (int)newPlayer.CustomProperties["lv"];
        var userNickName = newPlayer.NickName;

        go.Setup(userLevel, userNickName);
        //go.GetComponent<TMPro.TextMeshProUGUI>().text = $"{newPlayer.NickName} (LV.{userLevel})";
    }
    public void LeftrPlayer(Player otherPlayer)
    {
        if(_playerDic.ContainsKey(otherPlayer.ActorNumber))
        {
            Destroy( _playerDic[otherPlayer.ActorNumber].gameObject);
            _playerDic.Remove(otherPlayer.ActorNumber);
        }
    }


    public void ClickButton()
    {
        _content.gameObject.SetActive(!_content.gameObject.activeSelf);
    }
   }
