using System.Linq;

using Photon.Pun;
using UnityEngine;


public class PlayerInventory : MonoBehaviourPun
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform _bag;
    [Header("Listening")]
    [SerializeField] private EachInputControllerEventSO _addInputContollerEventSO;
    [SerializeField] private EachInputControllerEventSO _removeInputContollerEventSO;
    [SerializeField] private EachInputControllerEventSO _useSucessInputContollerEventSO;
    [SerializeField] private EachVoidEventChannelSO _onControllerChangeToAIEvent;

    private InputControllerObject[] _inputControllerObjects = new InputControllerObject[3];

    #region Life Cycle
    private void OnEnable()
    {
        _addInputContollerEventSO.onEventRaised += AddInputController;
        _removeInputContollerEventSO.onEventRaised +=RemoveItem;
        _useSucessInputContollerEventSO.onEventRaised += OnSucessInputController;
        _onControllerChangeToAIEvent.onEventRaised +=OnControllerChangeToAI;
    }
    private void OnDisable()
    {
        _addInputContollerEventSO.onEventRaised -= AddInputController;
        _removeInputContollerEventSO.onEventRaised -= RemoveItem;
        _useSucessInputContollerEventSO.onEventRaised -= OnSucessInputController;
        _onControllerChangeToAIEvent.onEventRaised -= OnControllerChangeToAI;
    }
    #endregion


    #region public
    public InputControllerObject GetInputController(int index)
    {
        return _inputControllerObjects[index] ?? null;
    }
    #endregion
    #region CallBack
    private void AddInputController(InputControllerObject newInputControllerObject)
    {
        var itemSO = newInputControllerObject.GetItemSO();
        var itemType = newInputControllerObject.GetItemType();
        int initItemIndex = (int)InputDefine.InputType.Item1;
        int selectIndex = -1;
        if (itemType == ItemType.Main)
        {
            selectIndex = 0;
        }
        else
        {
            for (int i = initItemIndex; i < _inputControllerObjects.Length; i++)
            {
                if (_inputControllerObjects[i] == null)
                {
                    selectIndex = i;
                    break;
                }
            }
            //여기까지왓다는건 모든아이템가방이 꽉찻을경우
            selectIndex = initItemIndex;
        }

        //범위를 벗어나면 실행x
        if (selectIndex < (int)InputDefine.InputType.Main || selectIndex > (int)InputDefine.InputType.Item2)
            return;
        CheckExisttem(selectIndex); //체크 현재 있는지 
        _inputControllerObjects[selectIndex] = newInputControllerObject;
        newInputControllerObject.transform.ResetTransform(this._bag);
        if (this.IsMyCharacter())
        {
            AddUIController(selectIndex, newInputControllerObject);
        }
    }
    private void RemoveItem(InputControllerObject inputControllerObject)
     {
        int findIndex = GetFireCode(inputControllerObject);
        if (findIndex == -1)
            return;
        _inputControllerObjects[findIndex] = null;
        if (this.IsMyCharacter())
        {
            RemoveUIController(findIndex);
        }
    }

    /// <summary>
    /// 소모품이라면 사용후 삭제
    /// </summary>
    private void OnSucessInputController(InputControllerObject inputControllerObject)
    {
        var itemSO = inputControllerObject.GetItemSO();
        if (itemSO.IsConsume)
            RemoveItem(inputControllerObject);
    }
    private void OnControllerChangeToAI()
    {
        int fireCode = -1;
        foreach(var inputObject in _inputControllerObjects)
        {
            fireCode++;
            if (inputObject == null)
                continue;
            if (inputObject.photonView.IsMine)
            {
                RemoveUIController(fireCode);
            }
            inputObject.photonView.ControllerActorNr = 0;
        }
    }
    #endregion

    #region private


    private void CheckExisttem(int index)
    {
        var currentInputController = _inputControllerObjects[index];
        if (currentInputController)
        {
            RemoveItem(currentInputController);
        }
    }


    private int GetFireCode(InputControllerObject inputControllerObject)
    {
        int findIndex = 0;
        for (int i = 0; i < _inputControllerObjects.Length; i++)
        {
            var checkInput = _inputControllerObjects[i];
            if (checkInput == null)
                continue;
            if (checkInput == inputControllerObject)
            {
                findIndex = i;
                return findIndex;
            }
        }
        return -1;
    }
    #endregion

    private void AddUIController(int fireCode , InputControllerObject inputControllerObject)
    {
        var itemSO = inputControllerObject.GetItemSO();
        _inputReader.AddUIController(fireCode,itemSO);
        if(itemSO.ZoomPrefab != null)
        {
            var zoom = Instantiate(itemSO.ZoomPrefab).GetComponent<UI_ZoomBase>();
            zoom.transform.ResetTransform(this.transform);
            zoom.Setup((InputDefine.InputType)fireCode, inputControllerObject.gameObject);
        }
    }

    private void RemoveUIController(int fireCode)
    {
        _inputReader.RevmoeUIController(fireCode);
    }
    //private void SetupLocalPlayer(int fireCode, bool active, InputControllerObject inputControllerObject = null)
    //{
    //    var inputType = (InputDefine.InputType)fireCode;
    //    if (inputControllerObject == null)
    //    {
    //        _inputReader.SetUIController(inputType, active);
    //        return;
    //    }
    //    var itemSO = inputControllerObject.GetItemSO();
    //    _inputReader.SetUIController(inputType, active, itemSO);
    //    if (active && itemSO.ZoomPrefab != null)
    //    {
    //        var zoom = Instantiate(itemSO.ZoomPrefab).GetComponent<UI_ZoomBase>();
    //        zoom.transform.ResetTransform(this.transform);
    //        zoom.Setup(inputType, inputControllerObject.gameObject);
    //    }
    //}


}
