using Data;
using ExitGames.Client.Photon;
using Photon.Pun;

public class PlayerController : PhotonRoomObjectController
{

    //public event Action<PhotonMessageInfo> onPhotonInstantiate;
    //public event Action<PhotonView> onPhotonDestroy;
    public PlayerControllerInfo PlayerInfo { get; set; }
    public Define.Team Team => PlayerHealth.Team;

    public PlayerInput PlayerInput { get; private set; }
    public PlayerHealth PlayerHealth { get; private set; }
    public PlayerShooter PlayerShooter { get; private set; }
    public PlayerMove PlayerMove { get; private set; }
    public PlayerAvater PlayerAvater { get; private set; }

    public PlayerUI PlayerUI { get; set; }

    public FogOfWarController FogOfWarController => PlayerHealth.FogOfWarController;
    public HideInFogController HideInFogController => PlayerHealth.HideInFogController;


    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        PlayerHealth = GetComponent<PlayerHealth>();
        PlayerMove = GetComponent<PlayerMove>();
        PlayerShooter = GetComponent<PlayerShooter>();
        PlayerAvater = GetComponent<PlayerAvater>();
        PlayerHealth.onDeath += HandleDeath;
    }
    private void OnEnable()
    {
        Managers.Game.AddPlayerController(this.ViewID(), this.GetComponent<PlayerController>());

    }
    private void OnDisable()
    {
        Managers.Game.RemovePlayerController(this.ViewID());
    }
    #region Call Back Event
    protected override void OnPhotonInit(PhotonMessageInfo info)
    {
        SetupPlayerInfo((Hashtable)info.photonView.InstantiationData[1]);

        if (this.gameObject.IsValidAI())
            return;
        //var controllerNr = (int)info.photonView.InstantiationData[0];
        var controllerNr = this.photonView.ControllerActorNr;
        var userController = Managers.Game.GetUserController(controllerNr);
        if (userController != null)
        {
            userController.playerController = this;
        }
        if (this.IsMyCharacter())
        {
            Managers.CameraManager.cameraState = Define.CameraState.MyPlayer;
        }
    }
    void SetupPlayerInfo(Hashtable HT)
    {
        var playerInfo = new PlayerControllerInfo ();
        if ( HT.TryGetValue("nn",out var nickName)){
            playerInfo.nickName = (string)nickName;
        }
        playerInfo.level = 1;
        PlayerInfo = playerInfo;
    }
    protected override void OnPhotonDestroy(PhotonView photonView)
    {
        if (this.gameObject.IsValidAI())
            return;
        var userController = Managers.Game.GetUserController(this.photonView.ControllerActorNr);
        if (userController != null)
        {
            userController.playerController = null;
        }
        if (this.IsMyCharacter())
        {
            Managers.CameraManager.cameraState = Define.CameraState.Auto;
        }
    }
    #endregion
    protected virtual void SetActiveEnable(bool active)
    {
        PlayerInput.enabled = false;
        PlayerHealth.enabled = false;
        PlayerMove.enabled = false;
        PlayerShooter.enabled = false;
        PlayerAvater.enabled = false;
    }



    void HandleDeath()
    {
        SetActiveEnable(false);

        Invoke("AfterDestory", 3.0f);
    }
    void AfterDestory()
    {
        Managers.Resource.PunDestroy(this);
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    var enterTrigger = other.gameObject.GetComponent<ICanEnterTriggerPlayer>();
    //    if (enterTrigger != null)
    //    {
    //        enterTrigger.Enter(this, other);
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    var exitTrigger = other.gameObject.GetComponent<ICanExitTriggerPlayer>();
    //    if (exitTrigger != null)
    //    {
    //        exitTrigger.Exit(this, other);
    //    }
    //}




 
    //public void OnCallBackTriggerEnter(Collider other)
    //{
    //    var enterTrigger = other.gameObject.GetComponent<ICanEnterTriggerPlayer>();
    //    if (enterTrigger != null)
    //    {
    //        enterTrigger.Enter(this, other);
    //    }
    //}
}

