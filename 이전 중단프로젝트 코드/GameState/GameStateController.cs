using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon.StructWrapping;
using WebSocketSharp;

public class GameStateController : MonoBehaviourPun, IPunInstantiateMagicCallback, IOnPhotonViewPreNetDestroy
{
    float _initRemainTime;
    float _createServerTime;
    int _remainTime;
    bool _isPlay = false;  //다음단계 넘어갔는지 안넘어갔는지
    [SerializeField] GameState_Base _gameState;

    public GameState_Base ttt => _gameState;
    Define.GameState _gameStateType;
    protected bool isNextScene;
    public Define.GameState gameStateType
    {
        get => _gameStateType;
        set
        {
            Component[] componetArray = GetComponents<GameState_Base>();
            foreach(var componet in componetArray)
            {
                Destroy(componet);
            }
            GameState_Base newGameState = null;
            switch (value)
            {
                case Define.GameState.Wait:
                    newGameState = this.gameObject.AddComponent<GameState_Wait>();
                    break;
                case Define.GameState.CountDown:
                    newGameState = this.gameObject.AddComponent<GameState_Count>();
                    break;
                case Define.GameState.GameReady:
                    newGameState = this.gameObject.AddComponent<GameState_GameReady>();
                    break;
                case Define.GameState.Gameing:
                    newGameState = this.gameObject.AddComponent<GameState_Gameing>();
                    break;
                case Define.GameState.End:
                    newGameState = this.gameObject.AddComponent<GameState_End>();
                    break;
            }
            _gameState = newGameState;
            _gameStateType = value;
            _initRemainTime = _gameState.remainTime;
            Managers.Game.gameStateController = this;
        }
    }

    public int remainTime
    {
        get => _remainTime;
        set
        {
            //Mathf.Abs( remainTime - _initRemainTime) <= 1
            if (_remainTime == value   ) return;
            _remainTime = value;

            _gameState.OnUpdate(_remainTime);
            if (_remainTime <= 0)
            {
                TimeEnd();
            }

        }
    }

    public object[] Datas { get; private set; }

    private void Awake()
    {
        _gameState = GetComponent<GameState_Base>();
    }

    public void ChangeInitTime(float time)
    {
        _initRemainTime = time; 
    }
    private void OnEnable()
    {
        StartCoroutine(Timer());

    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (info.photonView.InstantiationData == null) return;
        Datas = info.photonView.InstantiationData;
        gameStateType = (Define.GameState)Datas[0];
        _createServerTime = (float)info.SentServerTime;
        _isPlay = true;
        _gameState.OnPhotonInstantiate(info);
    }
    public void OnPreNetDestroy(PhotonView rootView)
    {
        _gameState.OnPhotonDestroy();
    }
    //private void Update()
    //{
    //    if (_createServerTime == 0 ) return;
    //    remainTime = (int)((_initRemainTime + _createServerTime) - (float)PhotonNetwork.Time);
    //}

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.5f);
        var waitSeconds = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return waitSeconds;
            remainTime = (int)((_initRemainTime + _createServerTime) - (float)PhotonNetwork.Time);
        }

    }

    void TimeEnd()
    {
        if (_isPlay == false) return;
        _isPlay = false;
        _gameState?.OnTimeEnd();
    }

    public void NextScene(Define.GameState gameState, object whoCanWin = null) => _gameState.NextScene(gameState, whoCanWin);
   

    
}
