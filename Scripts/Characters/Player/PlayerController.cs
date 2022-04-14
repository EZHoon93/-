
using ExitGames.Client.Photon;

using Photon.Pun;
using FoW;
using UnityEngine;
using UnityEngine.Events;
using BehaviorDesigner.Runtime;
using Photon.Realtime;

[System.Serializable]
public class TeamEvent : UnityEvent<Define.Team> { }
[System.Serializable]
public class PhotonInstantiateEvent : UnityEvent<PhotonMessageInfo> { }


public class PlayerController : MonoBehaviourPun, IPunInstantiateMagicCallback, IOnPhotonViewPreNetDestroy , IOnPhotonViewControllerChange
{
    #region members
    [Header("Varaible")]
    [SerializeField] private SkinContainerSO _characterSkinContainerSO;
    //[SerializeField] private FogOfWarTeam _fogOfWarTeamPrefab;
    [Header("Set")]
    [SerializeField] private TeamRunTimeSet _teamRunTimeSetSO;
    [SerializeField] private PlayerRuntimeSet _playerRuntimeSet;
    [Header("Listening")]
    [SerializeField] private EachVoidEventChannelSO _onDieEventSO;
    [SerializeField] private EachInputControllerEventSO _useSucessInputControllerEventSO;   //아이템및 스킬등 사용 콜백
    //[SerializeField] private 
    [Header("Broad Casting")]
    [SerializeField] private TransformEventChannelSO _cameraViewChannelSO;
    [SerializeField] private PhotonEventChannelSO _globalNoticePlayerDie;   //유저들에게 플레이어 사망알림
    [SerializeField] private EachPhotonViewEventChannelSO _OnPhotonInstantiateEventSO; //IPunInstantiateMagicCallback 콜백 시 발생
    [SerializeField] private EachPhotonViewEventChannelSO _OnPhotonDestroyEventSO; //IPunInstantiateMagicCallback 콜백 시 발생

    public UnityEvent<int> onTeamChange;

    private EntityAnimator _entityAnimator;
    private Damageable _damageable;
    private BehaviorTree _behaviorTree;

    #endregion

    #region Properties

    public Vector3 AttackDirection { get; private set; }
    public bool IsAttack { get; set; }
    public int ControllerNr { get; set; }

    public Define.Team Team => _damageable.Team;
    public bool IsDead => _damageable.IsDead;
    #endregion
    #region Life Cycle

    private void Awake()
    {
        _entityAnimator = GetComponent<EntityAnimator>();
        _damageable = GetComponent<Damageable>();
        _behaviorTree = GetComponent<BehaviorTree>();
    }

    private void OnEnable()
    {
        _playerRuntimeSet.Add(this);
        _onDieEventSO.onEventRaised += HandleDeath;
        _useSucessInputControllerEventSO.onEventRaised += SucessInputController;
    }

    private void OnDisable()
    {
        _playerRuntimeSet.Remove(this);
        _teamRunTimeSetSO.Remove(this);
        _onDieEventSO.onEventRaised -= HandleDeath;
        _useSucessInputControllerEventSO.onEventRaised -= SucessInputController;
        _OnPhotonDestroyEventSO.RaiseEvent(this.ViewID(), this.photonView);
    }
    #endregion



    #region public

    #endregion
    #region Call Back Event
  

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        ControllerNr  = (int)info.photonView.InstantiationData[0];
        //AI??..는 0이하
        ControllerCheck(ControllerNr);

        var dataHT = (Hashtable)info.photonView.InstantiationData[1];
        if (dataHT.TryGetValue(PunHashKeyConfig.team, out var team))
        {
            _teamRunTimeSetSO.Add(this);
            onTeamChange?.Invoke((int)team);
            SetupSkins();
        }
        
        _OnPhotonInstantiateEventSO.RaiseEvent(this.ViewID() , this.photonView);


        if (this.IsMyCharacter())
        {
            _cameraViewChannelSO.RaiseEvent(this.transform);
        }
    }
    public void OnPreNetDestroy(PhotonView rootView)
    {
        _OnPhotonDestroyEventSO.RaiseEvent(this.ViewID() , this.photonView);
    }


    private void HandleDeath()
    {
        _teamRunTimeSetSO.Remove(this);
        var killPlayerViewID = _damageable.LastDamagerViewID;
        var deathPlayerViewID = this.photonView.ViewID;

        if (photonView.IsMine)
        {
            _globalNoticePlayerDie.RaiseEventToServer(new object[] { killPlayerViewID, deathPlayerViewID });   //죽인 플레이어,죽은 플레이어 순
            Invoke("AfterDestroy", 2.0f);
        }
    }

    private void SucessInputController(InputControllerObject inputControllerObject)
    {
        var itemSO =  inputControllerObject.GetItemSO();
        if(string.IsNullOrEmpty( itemSO.UseAnimName) == false)
        {
            _entityAnimator.UpdateTriggerAniamtor(itemSO.UseAnimName);
            AttackDirection = inputControllerObject.UseInputVector;
        }
    }

    private void OnAttackStart()
    {
        IsAttack = true;
    }
    private void OnAttackEnd()
    {
        IsAttack = false;
    }
    #endregion

    #region private
    private void ControllerCheck(int number)
    {
        if (ControllerNr > 0)
        {
            this.photonView.ControllerActorNr = ControllerNr;
        }
        var isMine = ControllerNr == PhotonNetwork.LocalPlayer.ActorNumber ? true : false;
        _behaviorTree.enabled = isMine;
    }
    private void SetupSkins( )
    {
        var dataHT = (Hashtable)this.photonView.InstantiationData[1];
        var avaterKey = (int)dataHT["ch"];
        var characterAvater = _characterSkinContainerSO.CreateSkinObject(SkinType.Character, null).GetComponent<CharacterAvater>();
        characterAvater.onAttackStart += OnAttackStart;
        characterAvater.onAttackEnd += OnAttackEnd;

        if (this.TryGetComponent(out EntityAnimator entityAnimator))
        {
            entityAnimator.SetupAniamtor(characterAvater);
        }
        GetComponent<HideInFog>().AddRender(characterAvater.gameObject);
    }

    private void AfterDestroy()
    {
        PhotonNetwork.Destroy(this.photonView);
    }

    public void OnControllerChange(Player newController, Player previousController)
    {
        print("OnController Change!!");
    }
    #endregion

}

