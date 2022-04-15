using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopUpManager : MonoBehaviour
{
    [Header("Listening")]
    [SerializeField] private PopUpEventSO _popUpEventSO;


    private Dictionary<int, GameObject> _popDic = new Dictionary<int, GameObject>();

    private void OnEnable()
    {
        _popUpEventSO.onEventRaised += OnPopUp;
    }

    private void OnDisable()
    {
        _popUpEventSO.onEventRaised -= OnPopUp;

    }

    private GameObject OnPopUp(UIPopUpSO popSo)
    {
        var code = popSo.Prefab.GetInstanceID();
        if(_popDic.TryGetValue(code , out var popUpObject) == false)
        {
            popUpObject = Instantiate(popSo.Prefab);
            popUpObject.transform.ResetTransform(this.transform);
            _popDic.Add(code, popUpObject);
        }

        popUpObject.GetComponent<Canvas>().overrideSorting = true;
        if(popSo.PopUpType == PoPType.Message)
        {
            var messageObject = popUpObject.GetComponent<UIPopCheckMessage>();
            var messageSO = popSo as UIPopMessageSO;
            messageObject.Setup(messageSO.Content, () => messageSO.ConfirmEventSO.RaiseEvent() );
        }

        popUpObject.gameObject.SetActive(true);


        return popUpObject;

    }
}
