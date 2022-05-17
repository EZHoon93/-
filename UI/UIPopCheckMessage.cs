
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
public class UIPopCheckMessage : UIPopBase
{
    [SerializeField] private BoolVaraibleSO _isKorea;
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private TextMeshProUGUI _contentText;

    event UnityAction onConfirmEvent;
    private void Awake()
    {
        _cancelButton.onClick.AddListener(OnClickCancel);
        _confirmButton.onClick.AddListener(OnClickConfirm);

    }
    private void OnDisable()
    {
        onConfirmEvent = null;
    }

    public void Setup(string content, UnityAction confirmEvent)
    {
        _contentText.text = content;
        onConfirmEvent += confirmEvent;
    }

    //void OnPopUp(string content, UnityAction confirmAction)
    //{
    //    _contentText.text = content;
    //    onConfirmEvent = confirmAction;
    //    SetActive(true);
    //}

    void OnClickConfirm()
    {
        onConfirmEvent?.Invoke();
        this.gameObject.SetActive(false);
    }

    void OnClickCancel()
    {
        this.gameObject.SetActive(false);
    }

    //void SetActive(bool active)
    //{
    //    GetComponent<Canvas>().enabled = active;
    //}
}
