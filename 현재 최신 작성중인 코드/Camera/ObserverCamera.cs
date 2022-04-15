using UnityEngine;

public class ObserverCamera : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] float speed = 5;
    [Header("Listening")]
    [SerializeField] private TransformEventChannelSO _changeCameraViewEventCannelSO;
    [Header("BroadCasting")]
    [SerializeField] private TransformEventChannelSO _changeCameraTagetEventSO;





    private void OnEnable()
    {
        _changeCameraViewEventCannelSO.onEventRaised += ChangeCameraView;

        _changeCameraTagetEventSO.RaiseEvent(this.transform);
        _inputReader.SetActiveMoveVector(true);
    }
    private void OnDisable()
    {
        _changeCameraViewEventCannelSO.onEventRaised -= ChangeCameraView;
    }
    void ChangeCameraView(Transform target)
    {
        var isEqual = Equals(this.transform, target);
        this.gameObject.SetActive(isEqual);
    }
    private void Update()
    {
        this.transform.position += Time.deltaTime * speed * _inputReader.MoveInputVector3.normalized;
    }

}
