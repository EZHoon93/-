using BehaviorDesigner.Runtime;

using ExitGames.Client.Photon;

using FoW;

using Photon.Pun;

using UnityEngine;

public class PlayerAvater : MonoBehaviourPun  , IPunObservable
{
    [SerializeField] SkinContainerSO _characterSkinContainerSO;
    Animator _animator;
    CharacterAvater _characterAvater;
    public Animator PlayerAnimator => _animator;

    public CharacterAvater CharacterAvater => _characterAvater;

    int _moveAnimationKey;
    SharedVector3 input;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        //GetComponent<IMovement>().onChangeMoveState += ChangeMoveState;
        var ss = GetComponent<BehaviorTree>();
        input=  ss.GetVariable("MoveVector3") as SharedVector3;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(input.Value);
        }
        else
        {
            input.Value =(Vector3) stream.ReceiveNext();

        }
    }
    //void ChangeMoveState(Movement.State state)
    //{
    //    switch (state)
    //    {
    //        case Movement.State.Idle:
    //            UpdateAnimatorMove(0.0f);
    //            break;
    //        case Movement.State.Run:
    //            UpdateAnimatorMove(1.0f);
    //            break;
    //    }
    //}
    public void UpdateAnimatorMove(float speed)
    {
        _animator.SetFloat(_moveAnimationKey, speed);
    }

    public void SetActiveAvater(bool active)
    {
        _characterAvater?.gameObject.SetActive(active);
    }
    public void OnMyPhotonInstantiate(PhotonMessageInfo info)
    {
        this.gameObject.SetActive(false);
        _animator.enabled = false;
        if (this.enabled == false)
        {
            return;
        }
        var HT = (Hashtable)info.photonView.InstantiationData[1];
        var avarerKey = (int)HT["ch"];
        //var prefqab = _characterSkinContainerSO.SkinOjbects[avarerKey];
        //print("생성 " + prefqab);
        //Instantiate(prefqab).transform.ResetTransform(this.transform);
        //_characterAvater =CreateAvaterByIndex(avarerKey);   //아바타 생성
        _animator.enabled = true;
        this.gameObject.SetActive(true);


    }
    public void SetupEquipment(Equipmentable newEquipment)
    {
        var tr = _characterAvater.GetSkinParentTransform(newEquipment.skinType);
        newEquipment.model.transform.ResetTransform(tr);
    }
    private void Update()
    {
        _animator.SetFloat("Speed", input.Value.sqrMagnitude);

        if (photonView.IsMine)
        {
            _animator.SetFloat("Speed", input.Value.sqrMagnitude);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            _animator.enabled = !_animator.enabled;
        }
    }

    //CharacterAvater CreateAvaterByIndex(int avarerKey)
    //{
    //    if (_characterAvater)
    //    {
    //        Destroy(_characterAvater.gameObject);
    //    }
    //    //var go  = Managers.Spawn.GetSkinByIndex(Define.ProductType.Avater, avarerKey,this.transform).GetComponent<CharacterAvater>();
    //    if(go.TryGetComponent<HideInFog>(out var hideInFog)) {
    //        //_hideInFogController.AddHideInFog(hideInFog);
    //    }


    //    _animator = go.GetComponent<Animator>();
    //    _moveAnimationKey = Animator.StringToHash("Speed");
    //    return go;
    //}

    private void OnAnimatorIK(int layerIndex)
    {
        print("IK11");
        //// 총의 기준점 gunPivot을 3D 모델의 오른쪽 팔꿈치 위치로 이동
        //gunPivot.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

        //// IK를 사용하여 왼손의 위치와 회전을 총의 오른쪽 손잡이에 맞춘다
        //playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        //playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        //playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        //playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);

        //// IK를 사용하여 오른손의 위치와 회전을 총의 오른쪽 손잡이에 맞춘다
        //playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        //playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        //playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        //playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }

   
}
