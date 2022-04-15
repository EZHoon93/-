
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using FoW;

public class PlayerUI : MonoBehaviourPun
{
    [SerializeField] HideInFog _hideInFogs;
    [SerializeField] UI_PlayerGround _uI_PlayerGround;
    [SerializeField] UI_HpSlider _uI_HpSlider;
    [SerializeField] TextMeshProUGUI _playerNameText;

    [Header("Setting")]
    [SerializeField] Color _hiderTeamColor;
    [SerializeField] Color _seekerTeamColor;
    [SerializeField] float _showUITimeByHit;

    PlayerController _targetPlayerController;
    Define.Team Team => _targetPlayerController.Team;

    float _reaminShowUITimeByHit; //맞을때 UI보여줄시간

    private void OnEnable()
    {
        Managers.CameraManager.onChangeTargetPlayer += ChangeCameraTarget;
    }

    private void OnDisable()
    {
        if(Managers.Game == null)
        {
            return;
        }
        Managers.CameraManager.onChangeTargetPlayer -= ChangeCameraTarget;
    }
    private void LateUpdate()
    {
        this.transform.position = _targetPlayerController.transform.position;
    }

    public void Setup(PlayerController playerController, PhotonMessageInfo info)
    {
        _targetPlayerController = playerController;
        if (playerController.TryGetComponent<PlayerHealth>(out var playerHealth ))
        {
            playerHealth.HideInFogController.AddHideInFog(_hideInFogs);
            playerHealth.onDamage += HandleDamage;
            playerHealth.onDeath += HandleDeath;
            _uI_HpSlider.Setup(playerHealth);
        }
        if(playerController.TryGetComponent<PhotonRoomObjectController>(out var roomObjectController))
        {
            roomObjectController.onPhotonDestroyEvent += OnPhotonDestroy;
        }

        var HT =  (Hashtable)info.photonView.InstantiationData[1];
        var nickName = (string)HT["nn"];
        _playerNameText.text = nickName;
        _uI_PlayerGround.gameObject.SetActive(false);
        Init();
    }
    void Init()
    {
        ChangeTeam(_targetPlayerController.Team);
        ChangeCameraTarget(null, Managers.CameraManager.TargetPlayer);
    }
    public void OnPhotonDestroy(PhotonView rootView)
    {
        Managers.Resource.Destroy(this.gameObject);
    }

    void HandleDeath()
    {
        SetActiveUI(true);
        Util.CallBackFunction(this, 1.0f, () => SetActiveUI(false));
    }

    void HandleDamage(LivingEntity livingEntity, int damage)
    {
        if (_reaminShowUITimeByHit <= 0)
        {
            _reaminShowUITimeByHit = _showUITimeByHit;
            StartCoroutine(ShowUIByHit());
        }
        else
        {
            _reaminShowUITimeByHit = _showUITimeByHit;
        }
    }

    IEnumerator ShowUIByHit()
    {
        if (_targetPlayerController.IsMyCharacter() == false)
        {
            SetActiveUI(true);
        }
        while (_reaminShowUITimeByHit > 0)
        {
            _reaminShowUITimeByHit -= Time.deltaTime;
            yield return null;
        }
        if (_targetPlayerController.IsMyCharacter() == false)
        {
            SetActiveUI(false);
        }
    }

    void ChangeUI(Define.Team team)
    {
        var uiLayerIndex =  UtillLayer.GetLayerByTeam(team);
        Util.SetLayerRecursively(this.gameObject, uiLayerIndex);
    }
   


    bool ChangeColor(Define.Team team)
    {
        Color color = team == Define.Team.Hide ? _hiderTeamColor : _seekerTeamColor;
        _uI_PlayerGround.ChangeColor(color);
        return true;
    }




    void SetActiveUI(bool active)
    {
        _playerNameText.gameObject.SetActive(active);
        _uI_HpSlider.SetActiveHealthUI(active);
    }
    

    #region CallBack Event

 
    public void ChangeCameraTarget(PlayerController prevPlayer, PlayerController viewPlayer)
    {
        if(viewPlayer == null)
        {
            return;
        }
        //카메라 타깃이 숨는팀이라면 Show.
        if (_targetPlayerController == viewPlayer)
        {
            SetActiveUI(true);
            return;
        }
        //이미 대미지를입어서 UI를 보여주는경우.
        if (_uI_HpSlider.IsHpUpdateProgressive)
            return;

        if (viewPlayer.Team == Define.Team.Hide)
        {
            if(_targetPlayerController.Team == Define.Team.Hide)
            {
                SetActiveUI(false);
            }
            else
            {
                SetActiveUI(true);
            }
        }
        else
        {
            if (_targetPlayerController.Team == Define.Team.Hide)
            {
                SetActiveUI(false);
            }
            else
            {
                SetActiveUI(true);
            }
        }
        

    }




    public void ChangeTeam(Define.Team team)
    {
        ChangeUI(team);
        ChangeColor(team);
    }


    #endregion
}
