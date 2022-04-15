using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class DamageableUI : MonoBehaviourPun
{

    #region Varaibles
    [SerializeField] Slider _hpSlider;
    [SerializeField] Slider _backHpSlider;
    [SerializeField] TextMeshProUGUI _hpText;
    [SerializeField] float _showUITimeByHit;

    [Header("Listening")]
    [SerializeField] private EachVoidEventChannelSO _onDamageEventSO;
    [SerializeField] private EachVoidEventChannelSO _onDieEventSO;
    [SerializeField] private EachVoidEventChannelSO _onUpdateUIEventSO;

    private bool _backHpHit;
    private HealthSO _healthSO;
    #endregion
    #region Properties
    #endregion
    #region Life Cycle
    private void OnEnable()
    {
        _backHpHit = false;
        if (_healthSO != null)
        {
            _hpSlider.maxValue = _healthSO.MaxHealth;
            _backHpSlider.maxValue = _healthSO.MaxHealth;
            _hpText.text = _healthSO.CurrentHealth.ToString();
        }
        _onDamageEventSO.onEventRaised += HandleOnDamage;
        _onUpdateUIEventSO.onEventRaised += HandleUpdateUI;
        _onDieEventSO.onEventRaised += HandleOnDie;
    }
    private void OnDisable()
    {
        _onDamageEventSO.onEventRaised -= HandleOnDamage;
        _onUpdateUIEventSO.onEventRaised -= HandleUpdateUI;
        _onDieEventSO.onEventRaised -= HandleOnDie;

    }
    public void SetupHealthSO(HealthSO healthSO)
    {
        _healthSO = healthSO;
    }
    #endregion
    #region Override, Interface
    #endregion

    #region public



    #endregion

    #region CallBack
    private void HandleUpdateUI()
    {
        _hpText.text = _healthSO.CurrentHealth.ToString();
        StartCoroutine(UpdateHp());
    }
    private void HandleOnDamage()
    {
        if (!_backHpHit)
        {
            StartCoroutine(UpdateBackHp());
        }
    }
    private void HandleOnDie()
    {

    }
    #endregion

    #region private

    private IEnumerator UpdateHp()
    {
        while (_hpSlider.value >= _healthSO.CurrentHealth + 1)
        {
            _hpSlider.value = Mathf.Lerp(_hpSlider.value, _healthSO.CurrentHealth, Time.deltaTime * 5f);
            yield return null;
        }
        _hpSlider.value = _healthSO.CurrentHealth;

    }
    private IEnumerator UpdateBackHp()
    {
        _backHpHit = true;
        _backHpSlider.value = _hpSlider.value;
        yield return new WaitForSeconds(.5f);
        while (_backHpSlider.value >= _healthSO.CurrentHealth + 1)
        {
            _backHpSlider.value = Mathf.Lerp(_backHpSlider.value, _healthSO.CurrentHealth, Time.deltaTime * 7);
            yield return null;
        }

        _backHpHit = false;
        _backHpSlider.value = _healthSO.CurrentHealth - 1;
    }
    #endregion



  

}
