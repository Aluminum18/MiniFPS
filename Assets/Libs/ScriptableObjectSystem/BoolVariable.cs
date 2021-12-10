using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoolVar", menuName = "ScriptableObjectSystem/BoolVariable")]
public class BoolVariable : BaseScriptableObjectVariable<bool>
{
    protected override bool IsSetNewValue(bool value)
    {
        return value != _value;
    }
}
