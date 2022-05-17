using UnityEngine;



public enum PoPType
{
    Base,
    Message
}
[CreateAssetMenu (menuName ="Container/PopUp")]
public class UIPopUpSO : DescriptionBaseSO
{
    [SerializeField] private GameObject _prefab;



    protected virtual PoPType _popType => PoPType.Base;

    public PoPType PopUpType => _popType;

    public GameObject Prefab => _prefab;

    public GameObject popInstanceObject { get; private set; }

    public float LastPopTime { get; set; }



    public void OnClick()
    {
        Pop();
    }
    
    public virtual GameObject Pop()
    {
        return UIManager.Instance.OnPopUp(this);
    }
    public void Destroy()
    {
        Destroy(popInstanceObject);
    }

    public GameObject GetPopUpObject()
    {
        if(popInstanceObject == null)
            popInstanceObject = Instantiate(_prefab);

        LastPopTime = Time.time;
        return popInstanceObject;
    }
}
