using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScriptableObjectVariable <T> : ScriptableObject
{
    [TextArea(1, 10)]
    public string Explanation;

    [SerializeField]
    protected T _value;
    [SerializeField]
    protected T _defaultValue;
    [SerializeField]
    protected ScriptableObjectValueInit _valueWhenInit;

    public delegate void OnValueChangedDel(T newValue);
    public event OnValueChangedDel OnValueChange;

    public T Value
    {
        get
        {
            return _value;
        }
        set
        {
            if (!IsSetNewValue(value))
            {
                return;
            }
            _value = value;

            OnValueChange?.Invoke(value);           
        }
    }

    protected virtual bool IsSetNewValue(T value)
    {
        return true;
    }

    private void OnEnable()
    {
        switch (_valueWhenInit)
        {
            case ScriptableObjectValueInit.KeepCurrentOnEnable:
                {
                    break;
                }
            case ScriptableObjectValueInit.UseDefault:
                {
                    _value = _defaultValue;
                    break;
                }
        }
    }
}

public enum ScriptableObjectValueInit
{
    KeepCurrentOnEnable = 0,
    UseDefault = 1
}
