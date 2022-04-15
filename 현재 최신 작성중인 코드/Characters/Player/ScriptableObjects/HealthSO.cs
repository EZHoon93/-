using Unity.Collections;

using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An instance of the health of a character, be it the player or an NPC.
/// The initial values are usually contained in another SO of type HealthConfigSO.
/// </summary>
[CreateAssetMenu(fileName = "PlayersHealth", menuName = "EntityConfig/Player's Health")]
public class HealthSO : ScriptableObject 
{
    [Tooltip("The initial health")]
    [SerializeField] [ReadOnly] private int _maxHealth;
    [SerializeField] [ReadOnly] private int _currentHealth;
    private int _viewID;
    public int MaxHealth => _maxHealth;
    public int ViewID => _viewID;
    public int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
        }
    }
    public void SetupViewID(int viewID)
    {
        _viewID = viewID;
    }
    public void SetMaxHealth(int newValue)
    {
        _maxHealth = newValue;
    }
    public void SetCurrentHealth(int newValue)
    {
        _currentHealth = newValue;
    }

    public void InflictDamage(int DamageValue)
    {
        _currentHealth -= DamageValue;
        //onDamageEvent?.Invoke();
    }

    public void RestoreHealth(int HealthValue)
    {
        _currentHealth += HealthValue;
        if (_currentHealth > _maxHealth)
            _currentHealth = _maxHealth;
    }
}
