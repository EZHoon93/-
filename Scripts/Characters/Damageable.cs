using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class Damageable : MonoBehaviourPun, IPunObservable 
{

    #region Varaibles
    [Header("Prefab")]
    [SerializeField] private DamageableUI _hpSlider;
    [Header("Health")]
    [SerializeField] private HealthConfigSO _healthConfigSO;
    [SerializeField] private DamageableRunTimeSet _damageableRunTimeSet;
    [Header("Listening")]
    [SerializeField] private EachIntEventChannelSO _recovecyEventSO;
    [Header("Broadcasting On")]
    [SerializeField] private EachVoidEventChannelSO _eachOnDamageEventSO;
    [SerializeField] private EachVoidEventChannelSO _eachOnDieEventSO;
    [SerializeField] private EachVoidEventChannelSO _onUpdateUIEventSO;


    private HealthSO _currentHealthSO;
    #endregion
    #region Properties
    public bool IsDead { get; set; }
    public int LastDamagerViewID { get; private set; }//마지막으로 대미지를 준 ViewID

    public Define.Team Team { get; set; }
    #endregion

    #region Life Cycle

    private void Awake()
    {
        if (_currentHealthSO == null)
        {
            _currentHealthSO = ScriptableObject.CreateInstance<HealthSO>();
            _currentHealthSO.SetupViewID(this.photonView.ViewID);
            _currentHealthSO.SetMaxHealth(_healthConfigSO.InitHealth);
            _currentHealthSO.SetCurrentHealth(_healthConfigSO.InitHealth);
        }
        if (_hpSlider != null)
        {
            _hpSlider.SetupHealthSO(_currentHealthSO);
        }
    }

    private void OnEnable()
    {
        _damageableRunTimeSet.Add(this);
        _currentHealthSO.SetCurrentHealth(_healthConfigSO.InitHealth);
        _recovecyEventSO.onEventRaised += RecoveyHeal;
        IsDead = false;

    }

    private void OnDisable()
    {
        _damageableRunTimeSet.Remove(this);
        _recovecyEventSO.onEventRaised -= RecoveyHeal;
    }
    #endregion
 

    #region public

    [PunRPC]
    public void RecoveyHeal(int amount)
    {
        if (photonView.IsMine)
        {
            _currentHealthSO.RestoreHealth(amount);
            _onUpdateUIEventSO.RaiseEvent(this.photonView.ViewID);
            photonView.RPC("RecoveyHeal", RpcTarget.Others, amount);
        }

    }

    [PunRPC]
    public void ReceiveAnAttack(int damagerViewId, int damage, Vector3 hitPoint, bool isRPC = false)
    {
        if (IsDead)
        {
            return;
        }

        if (isRPC)
        {
            _currentHealthSO.InflictDamage(damage);
            _eachOnDamageEventSO?.RaiseEvent(this.ViewID());
            LastDamagerViewID = damagerViewId;
        }

        if (photonView.IsMine == false)
            return;

        _currentHealthSO.InflictDamage(damage);
        _eachOnDamageEventSO.RaiseEvent(this.photonView.ViewID);
        _onUpdateUIEventSO.RaiseEvent(this.photonView.ViewID);
        LastDamagerViewID = damagerViewId;
        if (_currentHealthSO.CurrentHealth > 0)
        {
            photonView.RPC("ReceiveAnAttack", RpcTarget.Others, damagerViewId, damage, hitPoint, true);
        }
        else
        {
            photonView.RPC("Die", RpcTarget.All);
        }
    }


    [PunRPC]
    public void Die()
    {
        if (photonView.IsMine)
        {
        }
        _eachOnDieEventSO?.RaiseEvent(this.ViewID());
        IsDead = true;
    }

    public void ChangeTeam(int teamCode)
    {
        Team = (Define.Team)teamCode;
    }
    #endregion

    #region CallBack
    #endregion

    #region private

    #endregion



    #region  Override, Interface
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_currentHealthSO.CurrentHealth);
            stream.SendNext(IsDead);
        }
        else
        {
            var  n_health = (int)stream.ReceiveNext();
            if(_currentHealthSO.CurrentHealth != n_health )
            {
                _currentHealthSO.CurrentHealth = n_health;
                _onUpdateUIEventSO.RaiseEvent(this.photonView.ViewID);
            }

            bool n_dead = (bool)stream.ReceiveNext();
            if (IsDead != n_dead)
            {
                IsDead = n_dead;
            }
        }
    }
    #endregion

    //public Define.Team Team =>



}
