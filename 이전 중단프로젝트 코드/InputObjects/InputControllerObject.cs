using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;


/// <summary>
///  항상 photonroomObject로 생성, 
/// </summary>
public class InputControllerObject : PhotonRoomObjectController 
{
    #region Event
    public event InputHandler downHandlerCallBack;
    public event InputHandler dragHandlerCallBack;
    public event InputHandler upHandlerCallBack;
    public event Action<PhotonMessageInfo > instinateCallBack;
    public event Action<PhotonView> destroyCallBack;


    #endregion 
    [SerializeField] PhotonView _photonView;
    [SerializeField] private Sprite _sprite;
    [SerializeField] InputDefine.InputType _inputType;
    [SerializeField] InputDefine.UIType _uIType;
    [SerializeField] PlayerShooter.STATE _shooterState;
    [SerializeField] string animaorName;
    [SerializeField] float _initCoolTime;

    PlayerController _playerController;


    public Sprite sprite => _sprite;
    public PlayerShooter PlayerShooter => _playerController.PlayerShooter;
    public PlayerController PlayerController => _playerController;
    public InputDefine.InputType InputType => _inputType;
    public InputDefine.UIType UIType => _uIType;
    public PlayerShooter.STATE ShooterState => _shooterState;
    public float RemainCoolTime { get; set; }

    private void OnEnable()
    {
        RemainCoolTime = 0;
    }

    protected override void OnPhotonInit(PhotonMessageInfo info)
    {
        var HT = (Hashtable)info.photonView.InstantiationData[1];
        int playerViewID = GetPlayerViewID(info);
        var playerController = Managers.Game.GetPlayerController(playerViewID);
        if (playerController)
        {
            _playerController = playerController;
            _playerController.PlayerShooter.SetupInputControllerObject(this);
            instinateCallBack?.Invoke(info);
        }
    }
    protected override void OnPhotonDestroy(PhotonView rootView)
    {
        RemoveToPlayer();
    }


    public void RemoveToPlayer()
    {
        if (_playerController == null)
            return;
        _playerController?.PlayerShooter.RemoveInputControllerObject(this);
        //_playerController = null;
    }

    /// <summary>
    /// 플레이어 캐릭이라면, 해당 photonView의 값을 "vid"키에 추가.
    /// </summary>
    int GetPlayerViewID(PhotonMessageInfo info)
    {
        var HT = (Hashtable)info.photonView.InstantiationData[1];
        return HT.ContainsKey("vid") ? (int)HT["vid"] : info.photonView.ViewID ;
    }
    public void Down(Vector3 inputVector3)
    {
        if (_playerController.IsMyCharacter() == false)
        {
            return;
        }
        downHandlerCallBack?.Invoke(inputVector3);
    }

    public void Drag(Vector3 inputVector3)
    {
        if (_playerController.IsMyCharacter() == false)
        {
            return;
        }
        dragHandlerCallBack?.Invoke(inputVector3);
    }

    public void Up(Vector3 inputVector3)
    {
        if (_playerController.IsMyCharacter() == false)
        {
            return;
        }
        upHandlerCallBack?.Invoke(inputVector3);
    }


    public bool Use()
    {
        if (RemainCoolTime > 0)
        {
            return false;
        }
        RemainCoolTime = RemainCoolTime;
        StartCoroutine(UpdateCoolTime());
        return true;
    }

    IEnumerator UpdateCoolTime()
    {
        var seconds = new WaitForSeconds(1.0f);
        while (RemainCoolTime > 0)
        {
            if (RemainCoolTime > 1)
            {
                yield return seconds;
            }
            else
            {
                yield return null;
            }
        }

        RemainCoolTime = 0;
    }


}
