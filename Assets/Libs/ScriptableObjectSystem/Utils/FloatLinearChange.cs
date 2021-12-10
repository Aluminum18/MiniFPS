using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class FloatLinearChange : MonoBehaviour
{
    [SerializeField]
    private bool _autoChangeOnEnable;
    [SerializeField]
    private FloatVariable _refValueSO;
    [SerializeField]
    private float _refValue;
    [SerializeField]
    private FloatVariable _changedValue;
    [SerializeField][Tooltip("Unit per sec")]
    private float _changeSpeed = 1f;
    [SerializeField]
    private ChangeType _changeType;

    [SerializeField]
    private UnityEvent _onFinishChange;

    public enum ChangeType
    {
        Increase,
        Decrease
    }

    private IDisposable _changeStream;

    public void AutoChangeBySpeed()
    {
        if (_changeType == ChangeType.Increase)
        {
            AutoIncreaseBySpeed();
            return;
        }

        AutoDecreaseBySpeed();
    }

    private void AutoDecreaseBySpeed()
    {
        _changeStream?.Dispose();

        _changedValue.Value = _refValueSO == null ? _refValue : _refValueSO.Value;

        _changeStream = Observable.EveryUpdate().Subscribe(_ =>
        {
            if (_changedValue.Value <= 0f)
            {
                _changedValue.Value = 0f;
                _changeStream.Dispose();
                _onFinishChange.Invoke();
                return;
            }

            _changedValue.Value -= _changeSpeed * Time.deltaTime;
        });
    }

    private void AutoIncreaseBySpeed()
    {
        _changeStream?.Dispose();

        float targetValue = _refValueSO == null ? _refValue : _refValueSO.Value;
        _changedValue.Value = 0f;

        _changeStream = Observable.EveryUpdate().Subscribe(_ =>
        {
            if (_changedValue.Value >= targetValue)
            {
                _changedValue.Value = targetValue;
                _changeStream.Dispose();
                _onFinishChange.Invoke();
                return;
            }

            _changedValue.Value += _changeSpeed * Time.deltaTime;
        });
    }

    private void OnEnable()
    {
        if (!_autoChangeOnEnable)
        {
            return;
        }
        AutoChangeBySpeed();
    }
}
