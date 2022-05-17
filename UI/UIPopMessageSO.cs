using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Container/PopMessage")]

public class UIPopMessageSO : UIPopUpSO
{
    [Header("???? ?? ?????? ???? ???? SO???? ???????? ?????? ")]
    [SerializeField] private LocalizitionSO _localizitionSO;
    [Header("???? ?????????? ???? ??????")]
    [SerializeField] private VoidEventChannelSO _confirmEventSO;
    protected override PoPType _popType => PoPType.Message;

    public string GetContent(Lanugage lanugage) => _localizitionSO.GetContent(lanugage);
    public VoidEventChannelSO ConfirmEventSO => _confirmEventSO;

    public override GameObject Pop()
    {
        var go = base.Pop().GetComponent<UIPopCheckMessage>();
        go.Setup( _localizitionSO.GetContent(Lanugage.Kor) , () => _confirmEventSO.RaiseEvent());
        return base.Pop();
    }
}
