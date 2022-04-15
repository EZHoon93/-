using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Container/PopMessage")]

public class UIPopMessageSO : UIPopUpSO
{
    private string _content;
    [Header("확인 버튼클릭시 액션 이벤트")]
    [SerializeField] private VoidEventChannelSO _confirmEventSO;
    [Header("호출 할 메시지 내용 해당 SO에는 각언어별 메시지 ")]
    [SerializeField] private DescriptionBaseSO _descriptionBaseSO;
    protected override PoPType _popType => PoPType.Message;

    public string Content => _content;
    public VoidEventChannelSO ConfirmEventSO => _confirmEventSO;

}
