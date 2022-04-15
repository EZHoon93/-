using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using FoW;
using ExitGames.Client.Photon.StructWrapping;

public class PlayerAvater : MonoBehaviourPun , IOnPhotonInstantiate
{
    [SerializeField] HideInFogController _hideInFogController;
    Animator _animator;
    CharacterAvater _characterAvater;
    public Animator PlayerAnimator => _animator;

    public CharacterAvater CharacterAvater => _characterAvater;

    int _moveAnimationKey;

    private void Awake()
    {
        GetComponent<IMovement>().onChangeMoveState += ChangeMoveState;
    }

    void ChangeMoveState(Movement.State state)
    {
        switch (state)
        {
            case Movement.State.Idle:
                UpdateAnimatorMove(0.0f);
                break;
            case Movement.State.Run:
                UpdateAnimatorMove(1.0f);
                break;
        }
    }
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
        if (this.enabled == false)
        {
            return;
        }
        var HT = (Hashtable)info.photonView.InstantiationData[1];
        var avarerKey = (int)HT["ch"];
        _characterAvater =CreateAvaterByIndex(avarerKey);   //아바타 생성

    }
    public void SetupEquipment(Equipmentable newEquipment)
    {
        var tr = _characterAvater.GetSkinParentTransform(newEquipment.skinType);
        newEquipment.model.transform.ResetTransform(tr);
    }


    CharacterAvater CreateAvaterByIndex(int avarerKey)
    {
        if (_characterAvater)
        {
            Destroy(_characterAvater.gameObject);
        }
        var go  = Managers.Spawn.GetSkinByIndex(Define.ProductType.Avater, avarerKey,this.transform).GetComponent<CharacterAvater>();
        if(go.TryGetComponent<HideInFog>(out var hideInFog)) {
            _hideInFogController.AddHideInFog(hideInFog);
        }

        _animator = go.GetComponent<Animator>();
        _moveAnimationKey = Animator.StringToHash("Speed");
        return go;
    }

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
