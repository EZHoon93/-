using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

/// <summary>
///  PhotonView의 첫번째 데이터는 무조건 플레이어 ViewID!!
/// </summary>
public class InputControllerObject : MonobehavoirPunRoom 
{
    private static Dictionary<string, ItemConfigSO> _cachedItemDic = new Dictionary<string, ItemConfigSO>();

    #region Varaibles
    [SerializeField] private PlayerRuntimeSet _playerRuntimeSet;
    
    [AssetList(Path = "ScriptableObjects/Item")]
    [SerializeField] private ItemConfigSO[] _itemConfigSOContainer;

    [SerializeField] private Transform _fireTransfom;
    [Title("Listening")]
    [FoldoutGroup("Event")] [SerializeField] private VoidEventChannelSO _onSucessAddInventoryEventSO;  //플레이어 인벤토리 들어가기 성공하였을떄
    [FoldoutGroup("Event")] [SerializeField] private Vector3EventChannelSO _onPlayerUseItemEventSO; //유저 아이템 사용시
    [FoldoutGroup("Event")] [SerializeField] private VoidEventChannelSO _onPlayerDieEventSO; //유저 아이템 사용시
    [FoldoutGroup("Event")] [SerializeField] private VoidEventChannelSO _onCallDestroyEventSO; //유저 아이템 사용시

    [Title("BraodCasting")]
    [FoldoutGroup("Event")] [SerializeField] private ItemSOEventChannelSO _addItemEventSO;
    [FoldoutGroup("Event")] [SerializeField] private ItemSOEventChannelSO _removeItemEventSO;
    [FoldoutGroup("Event")] [SerializeField] private ItemSOEventChannelSO _useSucessItemEventSO;
    [FoldoutGroup("Event")] [SerializeField] private VoidEventChannelSO _onStartCoolTimeUI;



    private ItemSO _itemSO;
    private ItemConfigSO _currentItemConfigSO;

    #endregion

    #region Properties
    private int PlayerViewID => _targetPlayer.photonView.ViewID;
    private PlayerController _targetPlayer => _itemSO.PlayerController;
    private void Resets() => _itemSO.Reset();   //리셋
    private ItemActionBase[] _itemActions => _currentItemConfigSO.ItemActions;


    #endregion
 

    #region Life Cycle
    protected override void OnStart()
    {
        var datas = this.photonView.InstantiationData;
        var playerViewID = (int)datas[1];
        var itemKey = (string)datas[2];
        var itemType = (ItemType)datas[3];
        //소유자 없으면 파괴
        if (!GetPlayer(playerViewID,out var playerController)|| !GetItemConfigSO(itemKey,out _currentItemConfigSO))
        {
            Destroy();
            return;
        }
        if (_currentItemConfigSO.ModelPrefab != null)
        {
            Instantiate(_currentItemConfigSO.ModelPrefab, this.transform);
        }

        _itemSO = _currentItemConfigSO.GetItemSO(this.photonView.ViewID);
        _itemSO.PlayerController = playerController;
        _itemSO.IsConsume = false;
        _itemSO.ItemType = itemType;
        _itemSO.onCallDestroy += Destroy;
        this.photonView.ControllerActorNr = playerController.photonView.ControllerActorNr;
        this.gameObject.tag = playerController.gameObject.tag;

        

        Resets();
        _onSucessAddInventoryEventSO.AddListener(OnSucessPlayerInventroy, _itemSO.ViewID);
        _onPlayerUseItemEventSO.AddListener(Fire, _itemSO.ViewID); //유저 인풋 리슨
        _onPlayerDieEventSO.AddListener(OnPlayerDie, _itemSO.PlayerViewID);
        _addItemEventSO.RaiseEvent(_itemSO, PlayerViewID); //플레이어 인벤토리 추가 이벤트 
        foreach (var action in _itemActions)
            action.OnStartAction(this, _itemSO);
    }
    protected override void OnDestroy()
    {
        if (_itemSO == null || _currentItemConfigSO == null)
        {
            return;
        }
        _removeItemEventSO.RaiseEvent(_itemSO, this.photonView.ViewID); //플레이어 인벤토리 삭제 이벤트 

        _onSucessAddInventoryEventSO.RemoveListener(OnSucessPlayerInventroy, _itemSO.ViewID);
        _onPlayerUseItemEventSO.RemoveListener(Fire, _itemSO.ViewID);
        _onPlayerDieEventSO.RemoveListener(OnPlayerDie, _itemSO.PlayerViewID);
        _currentItemConfigSO.RemoveItemSO(_itemSO.ViewID);

        foreach (var action in _itemActions)
            action.OnEndAction(this,_itemSO);

        if (this.IsMyCharacter())
        {
            _inputReader.OnRemoveItemSO(_itemSO);
        }
    }

    protected override void ControllerToAI(int prevControllerNr)
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == prevControllerNr)
        {
            _inputReader.OnRemoveItemSO(_itemSO);
        }
    }


    #endregion

    #region OnEvent


    //사망시 이벤트 발생
    private void OnPlayerDie()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
        //foreach (var action in _itemActions)
        //    action.OnDieAction(_itemSO);
    }



    // 플레이어 인벤토리에 성공적으로 들어갔을시
    private void OnSucessPlayerInventroy()
    {
        if (this.IsMyCharacter())
        {
            _inputReader.OnAddItemSO(_currentItemConfigSO, _itemSO);
            if (_currentItemConfigSO.ZoomPrefab)
            {
                var zoomObject = Instantiate(_currentItemConfigSO.ZoomPrefab).GetComponent<InputControllerObjectUI>();
                zoomObject.Setup(_currentItemConfigSO, _itemSO, _targetPlayer.transform);
                zoomObject.transform.ResetTransform(this.transform);
            }
        }
    }


    #endregion

    #region public
    [PunRPC]
    public void OnSuccessFire(Vector3 targetPoint)
    {
        _itemSO.StartPoint = _targetPlayer.transform.position + new Vector3(0,.5f,0.3f);
        if (targetPoint == Vector3.zero)
            _itemSO.TargetPoint = _targetPlayer.transform.position;
        else
            _itemSO.TargetPoint = targetPoint;

        _useSucessItemEventSO.RaiseEvent(_itemSO, PlayerViewID);
        //_useSucessItemEventSO.RaiseEvent(data , _targetPlayer.ViewID());    //플레이어에게 이벤트 전송
        //아이템 효과 발생
        foreach (var action in _itemActions)
        {
            //action.UseOnServer(_itemSO);
            action.UseOnServer(this,_itemSO);
        }

        if (photonView.IsMine)
        {
            if (_itemSO.IsConsume)
                PhotonNetwork.Destroy(this.photonView);
            else
                StartCoroutine(UpdateCoolTime());
        }
    }
    #endregion

    #region CallBack
    #endregion

    #region private

    private bool GetPlayer(int playerViewID, out PlayerController playerController)
    {
        playerController = _playerRuntimeSet.GetItem(playerViewID);
        if(playerController == null)
        {
            return false;
        }
        return true;
    }

    private bool GetItemConfigSO(string itemKey, out ItemConfigSO itemConfigSO)
    {
        if (_cachedItemDic.TryGetValue(itemKey, out itemConfigSO) == false)
        {
            itemConfigSO = _itemConfigSOContainer.Single(x => x.name == itemKey);
            if (itemConfigSO == null)
            {
                return false;
            }
            _cachedItemDic.Add(itemKey, itemConfigSO);
        }
        return true;
    }


    private void Fire(Vector3 inputVector)
    {
        if (_itemSO.RemainTime != 0 || inputVector.sqrMagnitude == 0)
        {
            return;
        }
        var targetPoint = _targetPlayer.transform.position + inputVector * _itemSO.Distance;
        targetPoint.y = _targetPlayer.transform.position.y;
        photonView.RPC("OnSuccessFire", RpcTarget.All, targetPoint);
    }

    private IEnumerator UpdateCoolTime()
    {
        _itemSO.RemainTime = _itemSO.MaxCoolTime;
        _onStartCoolTimeUI.RaiseEvent();
        while (_itemSO.RemainTime > 0)
        {
            _itemSO.RemainTime -= Time.deltaTime;
            yield return null;
        }
        _itemSO.RemainTime = 0;
    }

    #endregion





}
