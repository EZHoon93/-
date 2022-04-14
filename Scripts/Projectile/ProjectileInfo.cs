using UnityEngine;
using System;
using UnityEngine.Events;

public class ProjectileInfo : MonoBehaviour
{
    protected int _usePlayerViewID;
    protected int _damage;
    protected Define.Team _team;
    protected Vector3 _endPoint;

    public UnityEvent _onPushEvent;

    Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Setup(int viewID, Define.Team team, int damage, Vector3 startPoint, Vector3 endPoint , UnityAction onPushAction)
    {
        _usePlayerViewID = viewID;
        _team = team;
        _damage = damage;
        _endPoint = endPoint;

        _rigidbody.AddForce(Vector3.zero);
    }

}
