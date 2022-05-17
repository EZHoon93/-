using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopUpManager : MonoBehaviour
{
    [Header("Listening")]
    [SerializeField] private PopUpEventSO _popUpEventSO;    //???? ???? ???? 
    [SerializeField] private VoidEventChannelSO _clearEventSO;  //?????? ??????


    private Dictionary<int, UIPopUpSO> _popDic = new Dictionary<int, UIPopUpSO>();

    private Lanugage lanugage;

    public  void ClickTestEng()
    {
        lanugage = Lanugage.Eng;
    }
    public void ClickTestKor()
    {
        lanugage = Lanugage.Kor;
    }

  

    /// <summary>
    /// ???? ???? ???????? ???????????? ????
    /// </summary>
    private void Clear()
    {
        foreach(var popUpSO in _popDic.Values)
        {
            if( Time.time - popUpSO.LastPopTime > 100)
            {
                popUpSO.Destroy();
            }
        }
    }

    //????
    public GameObject OnPopUp(UIPopUpSO callPopSo)
    {
        var code = callPopSo.GetInstanceID();
        GameObject popObject = null;
        if (_popDic.TryGetValue(code , out var popUpSO) == false)
        {
            popUpSO = callPopSo;
            _popDic.Add(code, popUpSO);
        }
        popObject = popUpSO.GetPopUpObject();
        popObject.transform.ResetTransform(this.transform);
        //popObject.GetComponent<Canvas>().overrideSorting = true;
        //if(popUpSO.PopUpType == PoPType.Message)
        //{
        //    var messageObject = popObject.GetComponent<UIPopCheckMessage>();
        //    var messageSO = popUpSO as UIPopMessageSO;

        //    messageObject.Setup(messageSO.GetContent(lanugage), () => messageSO.ConfirmEventSO.RaiseEvent() );
        //}

        popObject.gameObject.SetActive(true);


        return popObject;

    }
}
