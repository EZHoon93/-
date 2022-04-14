
using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class PlayerControllerUI : MonoBehaviourPun
{


    #region Varaibles
    [SerializeField] private TextMeshProUGUI _playerNameText;
    [SerializeField] private DamageableUI _damageableUI;
    [Header("Listening")]
    [SerializeField] private EachBoolEventChannelSO _setActivePlayerUIEventSO;
    [SerializeField] private EachVoidEventChannelSO _eachOnDamageEventSO;
    [SerializeField] private EachPhotonViewEventChannelSO _onPhotonInstantiateEventSO;
    //PlayerController
    private Define.Team _team;
    #endregion
    #region Properties
    #endregion

    #region Life Cycle
    private void OnEnable()
    {
        _setActivePlayerUIEventSO.onEventRaised += SetAcitve;
        _eachOnDamageEventSO.onEventRaised += OnDaamge;
        _onPhotonInstantiateEventSO.onEventRaised += OnPhotonInstantiate;
    }
    private void OnDisable()
    {
        _setActivePlayerUIEventSO.onEventRaised -= SetAcitve;
        _eachOnDamageEventSO.onEventRaised -= OnDaamge;
        _onPhotonInstantiateEventSO.onEventRaised -= OnPhotonInstantiate;
    }

    #endregion
    #region Override, Interface
    private void OnPhotonInstantiate(PhotonView photonView)
    {
        var dataHT = (Hashtable)photonView.InstantiationData[1];
        _playerNameText.text = (string)dataHT["nn"];
        _team = (Define.Team)dataHT["te"];
    }
    #endregion

    #region public
    #endregion

    #region CallBack

    #endregion

    #region private

    private void OnDaamge()
    {
        if (_team == Define.Team.Seek)
            return;
        StopAllCoroutines();
        StartCoroutine(ProcessOnDamage());
    }

    private IEnumerator ProcessOnDamage()
    {
        SetAcitve(true);
        yield return new WaitForSeconds(1.0f);
        SetAcitve(false);
    }

    private void SetAcitve(bool active)
    {
        _playerNameText.enabled = active;
        _damageableUI.GetComponent<Canvas>().enabled = active;
    }
    #endregion
}
