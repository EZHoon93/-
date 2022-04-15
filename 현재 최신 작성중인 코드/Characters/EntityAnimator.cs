
using Photon.Pun;

using UnityEngine;

enum MoveState
{
    Idle,
    Move
}

public class EntityAnimator : MonoBehaviourPun , IPunObservable
{



    #region Varaibles
    [SerializeField] RuntimeAnimatorController _runtimeAnimatorController;
    [Header("Listening")]
    [SerializeField] EachBoolEventChannelSO _setACtivePlayerAvaterEvent;

    private Animator _animator;
    private float _moveSpeed;
    private int _moveAnimatorParameterHashId;
    #endregion
    #region Properties
    public bool IsAttack { get; set; }
    public Vector3 AttackDirection { get; set; }
    #endregion

    #region Life Cycle
    private void OnEnable()
    {
        _setACtivePlayerAvaterEvent.onEventRaised += OnSetActivePlayerAvater;
    }
    private void OnDisable()
    {
        _setACtivePlayerAvaterEvent.onEventRaised -= OnSetActivePlayerAvater;
    }
    private void Start()
    {
        _moveAnimatorParameterHashId = Animator.StringToHash("Speed");
    }
    #endregion
    #region Override, Interface

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_moveSpeed);
        }
        else
        {
            _moveSpeed = (float)stream.ReceiveNext();
            UpdateMoveAnimator(_moveSpeed);
        }
    }

    #endregion

    #region public

    public void SetupAniamtor(CharacterAvater characterAvater)
    {
        characterAvater.gameObject.layer = this.gameObject.layer;
        characterAvater.transform.ResetTransform(this.transform);

        _animator = characterAvater.animator;
        _animator.runtimeAnimatorController = _runtimeAnimatorController;
    }

    public void UpdateMoveAnimator(float newValue)
    {
        //if (newValue == 0)
        //    _state = MoveState.Idle;
        //else
        //    _state = MoveState.Move;
        _moveSpeed = newValue;
        _animator?.SetFloat(_moveAnimatorParameterHashId, newValue);
    }

    [PunRPC]
    public void UpdateTriggerAniamtor(string triggerName)
    {
        _animator.SetTrigger(triggerName);
        if (photonView.IsMine)
        {
            this.photonView.RPC("UpdateTriggerAniamtor", RpcTarget.Others, triggerName);
        }
    }
    #endregion

    #region CallBack


    #endregion

    #region private
    private void OnSetActivePlayerAvater(bool active)
    {
        _animator.gameObject.SetActive(active);
    }
    #endregion



    //private void OnUseItemSucess(UseInfo useInfo)
    //{
    //    AttackDirection = useInfo.inputVector;
    //    IsAttack = useInfo.itemSO.IsAttack; //애니메이션
    //    if (IsAttack)
    //    {
    //        UpdateTriggerAniamtor("Attack");
    //    }
    //}




}
