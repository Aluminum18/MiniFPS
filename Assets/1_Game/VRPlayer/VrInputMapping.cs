using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class VrInputMapping : MonoBehaviour
{
    public void OnActivateValue(InputAction.CallbackContext context)
    {
        
        Debug.Log("activate value: " + context.ReadValue<float>());
    }

    public void OnActivate(InputAction.CallbackContext context)
    {
        Debug.Log("activate: " + context.ReadValue<bool>());
    }
}
