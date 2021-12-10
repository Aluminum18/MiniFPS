using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UserDragInput : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [Header("Reference - Read")]
    [SerializeField]
    private FloatVariable _dragSensitive;

    [Header("Config")]
    [SerializeField]
    private Transform _cameraYRotate;
    [SerializeField]
    private Transform _cameraXRotate;
    [SerializeField]
    private bool _invertDrag;

    private float _verticalDegreePerPixel;
    private float _horizonDegreePerPixel;
    private Vector2 _dragDelta = Vector2.zero;

    private IDisposable _countTouchTimeStream;

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        _dragDelta = _invertDrag ? -eventData.delta : eventData.delta;

        _cameraXRotate.Rotate(-_dragDelta.y * _verticalDegreePerPixel * Time.deltaTime, 0f, 0f);
        _cameraYRotate.Rotate(0f, _dragDelta.x * _horizonDegreePerPixel * Time.deltaTime, 0f);
    }

    public void SetInvertDrag(bool invert)
    {
        _invertDrag = invert;
    }

    private void CalculateDragDegreePerPixel(float sensitive)
    {
        _verticalDegreePerPixel = 360f / Screen.height * sensitive;
        _horizonDegreePerPixel = 360f / Screen.width * sensitive * 2f;
    }

    private void OnEnable()
    {
        CalculateDragDegreePerPixel(_dragSensitive.Value);
        _dragSensitive.OnValueChange += CalculateDragDegreePerPixel;
    }

    private void OnDisable()
    {
        _dragSensitive.OnValueChange -= CalculateDragDegreePerPixel;
    }
}
