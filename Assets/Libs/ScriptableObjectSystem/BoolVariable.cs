using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoolVar", menuName = "ScriptableObjectSystem/BoolVariable")]
public class BoolVariable : BaseScriptableObjectVariable<bool>
{
    public void SetValueFromInputSystem(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Value = context.ReadValue<float>() == 1 ? true : false;
    }

    protected override bool IsSetNewValue(bool value)
    {
        return value != _value;
    }
}
