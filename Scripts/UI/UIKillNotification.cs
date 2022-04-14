using System.Collections;

using EZPool;

using UnityEngine;
using UnityEngine.UI;
public class UIKillNotification : MonoBehaviour
{
    [Header("Varabiles")]
    [SerializeField] TransformPoolSO _killNoticePoolSO;
    [Header("Panel")]
    [SerializeField] private Transform _panel;
    [Header("Listening")]
    [SerializeField] private NotifyKillInfoEventSO _notifyKillInfoEventSO;

    WaitForSeconds timer =  new WaitForSeconds(2.0f);

    private void OnEnable()
    {
        _notifyKillInfoEventSO.onEventRaised += OnRecivePlayerKillInfo;
    }
    private void OnDisable()
    {
        _notifyKillInfoEventSO.onEventRaised -= OnRecivePlayerKillInfo;
    }

    public void OnRecivePlayerKillInfo(string killPlayerNickName, string deathPlayerNickName)
    {
        var go =  _killNoticePoolSO.Pop();
        go.GetComponent<UIKillNotice>().Setup(killPlayerNickName, deathPlayerNickName);
        go.ResetTransform(_panel);
        StartCoroutine(TimerProcessKillUI(go.transform));
    }

    IEnumerator TimerProcessKillUI(Transform killNoticeObject)
    {
        yield return timer;
        _killNoticePoolSO.Push(killNoticeObject);
    }
}
