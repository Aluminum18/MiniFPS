using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class FloatVariableToFillImage : MonoBehaviour
{
    [SerializeField]
    private FloatVariable _refFloat;
    [SerializeField]
    private float _maxValue;
    [SerializeField] [Tooltip("unit per sec")]
    private float _autoReduceSpeed;
    [SerializeField]
    private Image _targetImage;

    private IDisposable _reduceStream;

    public void AutoReduceFromRefValue()
    {
        _reduceStream?.Dispose();

        float currentValue = _refFloat.Value;
        _reduceStream = Observable.EveryUpdate().Subscribe(_ =>
        {
            if (currentValue <= 0f)
            {
                UpdateImageFill(0f);
                _reduceStream.Dispose();
                return;
            }

            currentValue -= _autoReduceSpeed * Time.deltaTime;
            UpdateImageFill(currentValue);
        });
    }

    private void OnEnable()
    {
        _refFloat.OnValueChange += UpdateImageFill;
    }

    private void OnDisable()
    {
        _refFloat.OnValueChange -= UpdateImageFill;
    }

    private void UpdateImageFill(float value)
    {
        _targetImage.fillAmount = value / _maxValue;
    }
}
