using Cinemachine;

using UnityEngine;
public class CameraManager : MonoBehaviour 
{
    #region Varaibles
    [Header("Varaibles")]
    [SerializeField] private PlayerRuntimeSet _playerRuntimeSet;
    //[SerializeField] private IntVariableSO _currentViewPlayerViewIDSO;
    [Header("Listening")]
    [SerializeField] private TransformEventChannelSO _changeCameraTagetEventSO;
    [SerializeField] private VoidEventChannelSO _onClickObserverNextPlayerEvent;
    [SerializeField] private VoidEventChannelSO _onChangeWaitStateEvent;
    [Header("BroadCasting")]
    [SerializeField] private IntEventChannelSO _onChangeCameraViewEvent;
    [Header("Layer")]
    [SerializeField] private LayerMask _waitLayer;
    [SerializeField] private LayerMask _playerLayer;
    //[Header("memeber")]
    //[SerializeField] private Transform _observerTransform;
    

    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineBasicMultiChannelPerlin _virtualCameraNoise;
    [SerializeField] private Camera _mainCamera;
    int _observerIndex = 0;
    #endregion

    #region Properties
    #endregion

    #region Life Cycle



    protected void Awake()
    {
        if (_virtualCamera == null)
            _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        if (_virtualCameraNoise == null)
            _virtualCameraNoise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        //Managers.Game.onChangeGameState+= OnCallBack_GameState;
        InitFieldOfView();
    }
    private void OnEnable()
    {
        _changeCameraTagetEventSO.onEventRaised += SetupFollowTarget;
        _onChangeWaitStateEvent.onEventRaised += OnChangeWaitState;
        _onClickObserverNextPlayerEvent.onEventRaised += OnClickObserverNextPlayer;
    }
    private void OnDisable()
    {
        _changeCameraTagetEventSO.onEventRaised -= SetupFollowTarget;
        _onChangeWaitStateEvent.onEventRaised -= OnChangeWaitState;
        _onClickObserverNextPlayerEvent.onEventRaised -= OnClickObserverNextPlayer;


    }
    #endregion

    #region public
    #endregion

    #region CallBack

    private void OnChangeWaitState()
    {
        _mainCamera.cullingMask = _waitLayer;
        print("OnChangwWaitState");
    }

    private void SetupFollowTarget(Transform target)
    {
      _virtualCamera.Follow = target.transform;
        int fogTeam = 0;
       if(target.TryGetComponent(out PlayerController playerController))
        {
            fogTeam = playerController.photonView.ViewID;
            _mainCamera.cullingMask = _playerLayer;
        }
       else
            _mainCamera.cullingMask = _waitLayer;

        //_currentViewPlayerViewIDSO.Value = fogTeam;
        _onChangeCameraViewEvent.RaiseEvent(fogTeam);
    }
    private void OnClickObserverNextPlayer()
    {
        PlayerController targetViewPlayer = null;
        var playerControllerList = _playerRuntimeSet.Items;
        _observerIndex = _observerIndex <= playerControllerList.Count -1 ? _observerIndex + 1 : 0;
        do
        {
            var player = playerControllerList[_observerIndex];
            if (player == null)
            {
                _observerIndex = _observerIndex <= playerControllerList.Count -1 ? _observerIndex + 1 : 0;
            }
            targetViewPlayer = player;
        } while (targetViewPlayer == null);

        SetupFollowTarget(targetViewPlayer.transform);
    }


    #endregion

    #region private


    private void InitFieldOfView()
    {
        var cam = Camera.main;
        float width = Screen.width;
        float height = Screen.height;
        float designRatio = 9.0f / 16;
        float targetRatio = height / width;
        float fov = cam.fieldOfView;
        float reusltFov = targetRatio * fov / designRatio;
        cam.fieldOfView = reusltFov;
        _mainCamera = Camera.main;
    }

    //private void ShakeCamera(float _time, float _ampltiude, float _frequency)
    //{
    //    _virtualCameraNoise.m_AmplitudeGain = _ampltiude;
    //    _virtualCameraNoise.m_FrequencyGain = _frequency;
    //    Invoke("ShakeCameraOff", _time);
    //}
    //private void ShakeCameraOff()
    //{
    //    _virtualCameraNoise.m_AmplitudeGain = 0.0f;
    //    _virtualCameraNoise.m_FrequencyGain = 0.0f;
    //}
    //private void ShakeCameraByPosition(Vector3 pos, float time, float ampltiude, float frequency)
    //{
    //    if (UtillGame.IsView(pos) == false) return;
    //    ShakeCamera(time, ampltiude, frequency);
    //}
    #endregion





}
