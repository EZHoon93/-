
using System.Collections;

using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine;
using EZPool;

[CreateAssetMenu(menuName = "Item/Action/ChangeObjectAction", fileName = "ChangeObjectAction ")]
public class ChangeObjectAction : ItemActionBase
{
    [SerializeField] private GameStateSO _gameStateSO;
    [SerializeField] private PhotonEventChannelSO _cachedChangeHideObjectEvent;  //?????????? 
    [SerializeField] private PrefabEventChannelSO _getHidePrefabEventSO;
    [SerializeField] private ParticlePoolSO _particlePoolSO;
    [SerializeField] private BuffPoolSO _buffPoolSO;
    [SerializeField] private LayerMask _buffLayer;
    [SerializeField] private float _buffRange;


    public override void OnStartAction(MyMonoBehaviourPun runner,ItemSO itemSO)
    {
        if (runner.photonView.IsMine)
        {
            var randomObjectIndex = Random.Range(0, _gameStateSO.HideModeChangePrefabList().Length);
            ChangeToServer(itemSO.PlayerViewID, randomObjectIndex);
        }
    }
   
    public override void UseOnServer(MyMonoBehaviourPun runner, ItemSO itemSO)
    {
        if (runner.photonView.IsMine)
        {
            var randomObjectIndex = Random.Range(0, _gameStateSO.HideModeChangePrefabList().Length);
            ChangeToServer(itemSO.PlayerViewID, randomObjectIndex);
        }
        _particlePoolSO.Pop(itemSO.TargetPoint);
        //_buffPoolSO.PopBuffInRange(itemSO.TargetPoint, _buffRange, itemSO.PlayerViewID, _buffLayer, itemSO.Team);
        _buffPoolSO.PopBuffInRange(itemSO.TargetPoint, _buffRange, itemSO.PlayerViewID, _buffLayer, Define.Team.Seek);

    }
    public override void OnDieAction(MyMonoBehaviourPun runner,ItemSO itemSO)
    {
        //ChangeToServer(itemSO.PlayerViewID , -1);
    }
    

    private void RemoveCahedEvent(int playerViewID)
    {
        _cachedChangeHideObjectEvent.RaiseEventRemoveCached(new Hashtable
        {
            [UtilPhoton.playerViewIDKey] = playerViewID
        });
    }
    private void ChangeToServer(int playerViewID ,  int randomObjectIndex)
    {
        RemoveCahedEvent(playerViewID);
        _cachedChangeHideObjectEvent.RaiseEventToServer(new Hashtable
        {
            [UtilPhoton.playerViewIDKey] = playerViewID,
            [UtilPhoton.hideObjectIndexKey] = randomObjectIndex
        });
    }


}
