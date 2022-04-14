
using UnityEngine;
using UnityEngine.Events;

public class CharacterAvater : MonoBehaviour
{

    [SerializeField] private Transform _accessoriesTransform;
    [SerializeField] private Transform _headTransform;
    [SerializeField] private Transform _rightHand;

    public Animator animator;
    public event UnityAction onAttackStart;
    public event UnityAction onAttackEnd;





    [ContextMenu("Setup")]
    public void Setup()
    {
        _headTransform = transform.MyFindChild("Head_Accessories_locator");
        _accessoriesTransform = transform.MyFindChild("Accessories_locator");
        _rightHand = transform.MyFindChild("WeaponR_locator");
    }
  
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.applyRootMotion = false;
    }

    private void OnDisable()
    {
        onAttackStart = null;
        onAttackEnd = null;
    }


    //애니메이션 이벤트
    public void OnAttackStart() => onAttackStart?.Invoke();

    public void OnAttackEnd() => onAttackEnd?.Invoke();

    
    public Transform GetSkinParentTransform(Define.SkinType skinType)
    {
        switch (skinType)
        {
            case Define.SkinType.RightHand:
                return _rightHand;
            case Define.SkinType.Hat:
                return _headTransform;
            case Define.SkinType.Bag:
                return _accessoriesTransform;
            case Define.SkinType.Skin:
                return this.transform;
        }

        return null;
    }

    public void SetupEquipment(Equipmentable newEquipment)
    {
        var tr = GetSkinParentTransform( newEquipment.skinType);

        newEquipment.model.transform.ResetTransform(tr);
    }

    public void SetupAccessories(GameObject newAccssories)
    {
        //if (newAccssories == null) return;
        //if (accessoriesObject)
        //{
        //    //Managers.Resource.Destroy(accessoriesObject.gameObject);
        //}
        //accessoriesObject = newAccssories;
        //accessoriesObject.transform.ResetTransform(accessoriesTransform.transform);
    }

    public void SetupWeapon(GameObject newWeapon)
    {
        if(newWeapon == null)return;
        //if (weaponObject)
        //{
        //    //Managers.Resource.Destroy(weaponObject.gameObject);
        //}
        //weaponObject = newWeapon;
        //weaponObject.transform.ResetTransform(rightHand.transform);
    }

    // 애니메이터의 IK 갱신
    private void OnAnimatorIK(int layerIndex)
    {
        print("IK3");
        //_playerController.playerShooter.OnAnimatorIK(layerIndex);
    }
}
