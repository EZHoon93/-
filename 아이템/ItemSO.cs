using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu (menuName = "Datas/Item")]
public class ItemSO : DescriptionBaseSO
{
    public UnityAction onCallDestroy;

    public int ViewID { get; set; }
    public PlayerController PlayerController { get; set; }

    public ItemConfigSO ItemConfigSO { get; set; }

    public ItemType ItemType {get;set;}
    public Define.Team Team
    {
        get => PlayerController.Team;
        set
        {

        }
    }
    public int PlayerViewID 
    {
        get
        {
            if (PlayerController == null)
                return 0;

            return  PlayerController.photonView.ViewID;
        }
    }
    public int FireCode
    {
        get; set;
    }
    
    public bool IsConsume
    {
        get;
        set;
    }
 
    public float RemainTime
    {
        get; 
        set;
    }
    public float Distance
    {
        get;
        set;
    }
    public float MaxCoolTime
    {
        get;
        set;
    }
    public Vector3 TargetPoint 
    {
        get; 
        set; 
    }

    public Vector3 StartPoint
    {
        get;
        set;
    }
   
    public virtual void Reset()
    {
        RemainTime = 0;
        Distance = ItemConfigSO.InitDistacne;
        MaxCoolTime = ItemConfigSO.InitCoolTime;
    }

    public void Destroy()
    {
        onCallDestroy?.Invoke();
    }


}
