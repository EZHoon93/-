using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;


//[CreateAssetMenu(menuName = "Variable/T")]
public class VariableSO<T> : ScriptableObject
{
    [SerializeField]
    protected T _value;
    public virtual T Value
    {
        get => _value;
        set
        {
            if (object.Equals(_value, value))
                return;
            _value = value;
            OnChangeValue?.Invoke();
            OnResponseEvent?.Invoke(value);
        }
    }

    public UnityAction OnChangeValue;
    //sSerializable]
    public UnityEvent<T> OnResponseEvent;



    private void OnDestroy()
    {
        Debug.Log("OnDestroy VariableSO");
        _value = default ( T);
        //_value = null;
    }
}


