using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Container/PopMessage")]

public class UIPopMessageSO : UIPopUpSO
{
    private string _content;
    [Header("Ȯ�� ��ưŬ���� �׼� �̺�Ʈ")]
    [SerializeField] private VoidEventChannelSO _confirmEventSO;
    [Header("ȣ�� �� �޽��� ���� �ش� SO���� ���� �޽��� ")]
    [SerializeField] private DescriptionBaseSO _descriptionBaseSO;
    protected override PoPType _popType => PoPType.Message;

    public string Content => _content;
    public VoidEventChannelSO ConfirmEventSO => _confirmEventSO;

}
