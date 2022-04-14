
using ExitGames.Client.Photon;

using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class ChangeHideObjectItem : MonoBehaviourPun  , IPunInstantiateMagicCallback
{


    #region Varaibles
    [SerializeField] private GameStateSO _gameStateSO;
    [SerializeField] private PlayerRuntimeSet _playerRuntimeSet;
    [Header("Listeing")]
    [SerializeField] private EachVoidEventChannelSO _onDiePlayer;  //플레이어 사망시
    [Header("BroadCasting")]
    [SerializeField] private EachBoolEventChannelSO _setActivePlayerUIEventChannel;  //각플레이어 UI끔
    [SerializeField] private EachBoolEventChannelSO _setActiveCharacterIEventChannel;  //각플레이어 캐릭터 끔
    [Header("BroadCasting & Listeningl ")]
    [SerializeField] private PhotonEventChannelSO _cachedChangeHideObjectEvent;  //서버로부터 
    [Header("Varaibles")]
    [SerializeField] private ParticleSystem _dieEffectParticle;

    private GameObject _changeObject;
    #endregion
    #region Properties
    #endregion

    #region Life Cycle
    private void OnEnable()
    {
        _onDiePlayer.onEventRaised += HandleDie;
    }
    private void OnDisable()
    {
        _onDiePlayer.onEventRaised -= HandleDie;
    }
    #endregion
    #region Override, Interface
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (_changeObject)
        {
            Destroy(_changeObject);
        }
        var playerViewID = (int)info.photonView.InstantiationData[0];
        ChangeHideObject(playerViewID);
  
    }

    #endregion

    #region public
    #endregion

    #region CallBack
    public void CallBackCachedPhotonData(object data)
    {
        var dataHT = (Hashtable)data;
        if(dataHT.TryGetValue(PhotonKeyConfig.viewID ,out var viewID))
        {
            if (this.photonView.ViewID!= (int)viewID)
                return;
        }

        if (dataHT.TryGetValue(PhotonKeyConfig.hideObjectIndex , out var hideObjectIndex))
        {
            _changeObject = CreateHideObject((int)hideObjectIndex);
            _changeObject.layer = LayerMask.NameToLayer("Player");
        }
        ChangeCollider(_changeObject.GetComponent<NavMeshObstacle>());

        var playerViewID = (int)this.photonView.InstantiationData[0];
        _setActiveCharacterIEventChannel.RaiseEvent(playerViewID, false); //플레이어 캐릭터 끔
        _setActivePlayerUIEventChannel.RaiseEvent(playerViewID, false); //플레이어 UI끔
    }
    #endregion

    #region private

    private PlayerController GetPlayer()
    {
        var playerViewID = (int)this.photonView.InstantiationData[0];
        return _playerRuntimeSet.GetItem(playerViewID);
    }

    private GameObject[] HidePrefabArray()
    {
        var hideScene = _gameStateSO.CurrentGameSceneSO as HideSceneSO;
        if (hideScene == null)
            return null;
        return hideScene.HidePrefabs;
    }

    private GameObject CreateHideObject(int index)
    {
        var prefabArray = HidePrefabArray();
        if(index > prefabArray.Length)
        {
            Debug.LogError("Error Create Hide Object");
            return null;
        }
        var selectPrefab = prefabArray[index];
        var go = Instantiate(selectPrefab, this.transform);
        return go;
    }

    private void ChangeCollider(NavMeshObstacle navMeshObstacle)
    {
        if (navMeshObstacle == null)
            return;

        var playerController = GetPlayer();
        if (playerController == null)
            return;
        playerController.GetComponent<CapsuleCollider>().enabled = false;
        var boxCollider = playerController.gameObject.GetOrAddComponent<BoxCollider>();
        bool isCapsule;
        if (navMeshObstacle.shape == NavMeshObstacleShape.Capsule)
        {
            //_capsuleCollider.center = navMeshObstacle.center;
            //_capsuleCollider.radius = navMeshObstacle.radius;
            //_capsuleCollider.enabled = true;
            isCapsule = true;
        }
        else
        {
            boxCollider.center = navMeshObstacle.center;
            boxCollider.size = navMeshObstacle.size;
            isCapsule = false;
        }

        //_capsuleCollider.enabled = isCapsule;
        boxCollider.enabled = !isCapsule;
    }

    private void HandleDie()
    {
        _dieEffectParticle?.Play();
        _changeObject.SetActive(false);
        //this.gameObject.SetActive(false);
    }

    private void ChangeHideObject(int playerViewID)
    {
        if(photonView.IsMine == false)
        {
            return;
        }

        var randomObjectIndex = Random.Range(0, HidePrefabArray().Length);
        _cachedChangeHideObjectEvent.RaiseEventRemoveCached(new Hashtable {
            [PhotonKeyConfig.viewID] = this.photonView.ViewID
        });

        _cachedChangeHideObjectEvent.RaiseEventToServer(
            new Hashtable{
                [PhotonKeyConfig.viewID] = this.photonView.ViewID,
                [PhotonKeyConfig.hideObjectIndex] =  randomObjectIndex
        });
    }
    #endregion





}
