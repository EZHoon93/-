
using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System;

public class PhotonMove : MonoBehaviourPun  , IPunObservable
{
    #region Event;
    public event Action<MoveState> onChangeMoveStateEvent;
    #endregion
    public enum MoveState
    {
        Idle,
        Run,
        Stun,
    }
    MoveState _state;
    public MoveState State
    {
        get => _state;
        set
        {
            if (_state == value) return;
            _state = value;
            onChangeMoveStateEvent?.Invoke(_state);
        }
    }
    public DataState dataState;
    public enum DataState
    {
        SerializeView,
        ServerView
    }
    public bool CanRotation { get; set; }   //회전 가능성, false 면 이동만가능.


    [SerializeField] protected Transform _rotateTarget;

    protected float rotationSpeed = 8;
    private Vector3 networkPosition = Vector3.zero; //We lerp towards this
    private Vector3 oldPosition;
    private Vector3 movement;
    protected Vector3 n_direction;
    float m_distance;
    bool m_firstTake;
    public float SmoothingDelay = 5;



    List<float> _moveBuffRatioList = new List<float>(); //캐릭에 슬로우및이속증가 버퍼리스트
     protected float _totRatio;    //버퍼리스트 합계산한 최종 이속 증/감소율

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            WriteData(stream, info);
        }
        else
        {
            ReadData(stream, info);
        }
    }

    protected virtual void WriteData(PhotonStream stream, PhotonMessageInfo info)
    {
        stream.SendNext(n_direction);
        stream.SendNext(transform.position);
        stream.SendNext(movement);
        //stream.SendNext(State);
    }

    protected virtual void ReadData(PhotonStream stream, PhotonMessageInfo info)
    {
        n_direction = (Vector3)stream.ReceiveNext();
        networkPosition = (Vector3)stream.ReceiveNext();
        movement = (Vector3)stream.ReceiveNext();
        //State = (MoveState)stream.ReceiveNext();
        if (m_firstTake)
        {
            this.transform.position = networkPosition;
            m_firstTake = false;
        }
        else
        {
            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            networkPosition += movement * lag;
            m_distance = Vector3.Distance(this.transform.position, networkPosition);
        }
    }


    protected virtual void OnEnable()
    {
        m_firstTake = true;
        dataState = DataState.SerializeView;
        CanRotation = true;
    }

    public virtual void OnPhotonInstantiate()
    {

    }
    public virtual void OnPreNetDestroy(PhotonView rootView)
    {

    }

    private void FixedUpdate()
    {
        if(photonView.IsMine == false)
        {
            return;
        }
        UpdateLocal();
        oldPosition = this.transform.position;
        networkPosition = this.transform.position;
        movement = networkPosition - oldPosition;
    }
    protected virtual void Update()
    {
        if (photonView.IsMine)
        {
            return;
        }
        if (dataState == DataState.ServerView) return;
        UpdateRemote();
        transform.position = Vector3.MoveTowards(transform.position, this.networkPosition, m_distance * Time.deltaTime * PhotonNetwork.SerializationRate);
    }
 

    protected virtual void UpdateLocal()
    {

    }

    protected virtual void UpdateRemote()
    {
     
    }


 
    protected virtual void UpdateSmoothRotate(Vector3 direction)
    {
        if (!CanRotation || direction.normalized.sqrMagnitude == 0)
        {
            n_direction = this._rotateTarget.forward;
            return;
        }
        direction.y = 0;
        n_direction = direction;
        Quaternion newRotation = Quaternion.LookRotation(n_direction);
        //this._rotateTarget.rotation = Quaternion.Slerp(this._rotateTarget.rotation, newRotation, rotationSpeed * Time.deltaTime);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, newRotation, rotationSpeed * Time.deltaTime);

    }

    protected void UpdateImmediateRotate(Vector3 direction)
    {
        if (n_direction == direction)
        {
            return;
        }
        //inputVector3.y = _rotateTarget.transform.position.y;
        Quaternion newRotation = Quaternion.LookRotation(direction);
        this._rotateTarget.rotation = newRotation;
        //this._rotateTarget.LookAt(inputVector3);
        n_direction = this._rotateTarget.forward;
    }

    public void AddMoveBuffList(float ratio, bool isAdd)
    {
        if (isAdd)
        {

            _moveBuffRatioList.Add(ratio);
        }
        else
        {
            _moveBuffRatioList.Remove(ratio);
        }

        _totRatio = 0; ;
        foreach (var v in _moveBuffRatioList)
        {
            _totRatio += v;
        }
    }



}
