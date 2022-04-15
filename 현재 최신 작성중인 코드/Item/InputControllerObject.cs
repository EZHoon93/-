using System.Collections;
using UnityEngine;
using Photon.Pun;

/// <summary>
///  PhotonView의 첫번째 데이터는 무조건 플레이어 ViewID!!
/// </summary>
public class InputControllerObject : MonoBehaviourPun , IPunInstantiateMagicCallback 
{
    #region Varaibles
    [SerializeField] private ItemSO _itemSO;
    [Header("Listening")]
    [Header("BroadCasting")]
    [SerializeField] private EachInputControllerEventSO _addInputContollerEventSO;
    [SerializeField] private EachInputControllerEventSO _removeInputContollerEventSO;
    [SerializeField] private EachInputControllerEventSO _useSucessInputContollerEventSO;

    private IInputObject _inputObject;
    private int _playerViewID;
    private float _remainCoolTime;
    #endregion

    #region Properties
    public virtual bool IsCanUse => _remainCoolTime == 0 ? true : false;

    public ItemSO GetItemSO() => _itemSO;
    public ItemType GetItemType() => _itemSO.ItemTypes;
    public Vector3 UseInputVector { get; private set; }
    #endregion

    #region Life Cycle

    private void Awake()
    {
        _inputObject = GetComponent<IInputObject>();
    }
    private void OnEnable()
    {
        _remainCoolTime = 0;
    }
    private void OnDisable()
    {
        _removeInputContollerEventSO.RaiseEvent(_playerViewID, this);
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        _playerViewID = (int)info.photonView.InstantiationData[0];
        _addInputContollerEventSO.RaiseEvent(_playerViewID, this);
    }
    #endregion

    #region public

    public void Fire(Vector3 inputVector)
    {
        if (_inputObject == null || IsCanUse == false || inputVector.sqrMagnitude == 0)
        {
            return;
        }
        UseInputVector = inputVector;
        _remainCoolTime = _itemSO.InitCoolTime;
        _inputObject.Use(_playerViewID, inputVector);
        _useSucessInputContollerEventSO.RaiseEvent(_playerViewID, this);    //이벤트 전송
        if (_itemSO.IsConsume == false)
        {
            StartCoroutine(UpdateCoolTime());
        }
    }
    #endregion

    #region CallBack
    #endregion

    #region private

    private IEnumerator UpdateCoolTime()
    {
        while (_remainCoolTime > 0)
        {
            _remainCoolTime -= Time.deltaTime;
            yield return null;
        }
        _remainCoolTime = 0;
    }

    #endregion





}
