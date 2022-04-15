using UnityEngine;



public enum PoPType
{
    Base,
    Message
}
[CreateAssetMenu (menuName ="Container/PopUp")]
public class UIPopUpSO : DescriptionBaseSO
{
    [Header("�˾�â ������")]

    [SerializeField] private GameObject _prefab;
//    [Header("BroadCasting")]
    [Header("�˾� �̺�Ʈ ȣ��")]

    [SerializeField] private PopUpEventSO _popUpEventSO;
    protected virtual PoPType _popType => PoPType.Base;

    public PoPType PopUpType => _popType;

    public GameObject Prefab => _prefab;

    public void OnClick()
    {
        _popUpEventSO.RaiseEvent(this);
    }
}
