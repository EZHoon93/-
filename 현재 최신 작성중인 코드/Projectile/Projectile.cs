using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public enum ProjectileType
{
    Bullet,   //직선
    Missile , //직선
    Grenade ,//포물선

}
public abstract class Projectile : MonoBehaviour 
{
    [SerializeField] protected ProjectileConfigSO _projectileConfigSO;
    [SerializeField] protected GameObject _projecitleModel;
    [SerializeField] protected ParticleSystem _effectParticle;

    public event UnityAction onPushEvent;

    protected int _playerViewID;
    protected Define.Team _team;
    protected int _damage;
    protected float _speed;
    protected Vector3 _endPoint;
    protected bool _isPlay;
    public GameObject PorjectileModel => _projecitleModel;
    public int PlayerViewID => _playerViewID;


    protected virtual void Awake()
    {
        //var mainModule = _effectParticle.main;
        //mainModule.playOnAwake = false;
    }
 

    public virtual void Setup(Define.Team team, int playerViewID, int damage, float speed , Vector3 endPoint , UnityAction onPushAction = null)
    {
        _playerViewID = playerViewID;
        _team = team;
        _damage = damage;
        _speed = speed;
        _endPoint = endPoint;

        _isPlay = true;
    }
    protected virtual void End()
    {
        _isPlay = false;
        onPushEvent?.Invoke();
        this.gameObject.SetActive(false);
    }

    
}
