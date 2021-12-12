using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeAdjust : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private FloatVariable _scopeZoomValue;
    [SerializeField]
    private CinemachineVirtualCamera _scopeCamera;

    private void UpdateScope(float scopeValue)
    {
        _scopeCamera.m_Lens.FieldOfView = Mathf.Lerp(15f, 2f, _scopeZoomValue.Value);
    }

    private void OnEnable()
    {
        UpdateScope(_scopeZoomValue.Value);
        _scopeZoomValue.OnValueChange += UpdateScope;
    }

    private void OnDisable()
    {
        _scopeZoomValue.OnValueChange -= UpdateScope;
    }
}
