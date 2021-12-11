using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class MoveForwardToByTransform : MonoBehaviour
{
    [Header("Reference - Read")]
    [SerializeField]
    private Vector3Variable _headTo;

    [SerializeField]
    private UnityEvent _onStartMove;
    [SerializeField]
    private UnityEvent _onStopMove;

    [Header("Config")]
    [SerializeField]
    private bool _moveOnEnable;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private Transform _moveTransform;

    private IDisposable _moveStream;

    public void StartMove()
    {
        if (_moveStream != null)
        {
            _moveStream.Dispose();
        }

        _moveTransform.rotation = Quaternion.LookRotation(_headTo.Value - _moveTransform.position);

        _onStartMove.Invoke();
        _moveStream = Observable.EveryUpdate().Subscribe(_ =>
        {
            _moveTransform.position += _moveTransform.forward * _speed * Time.deltaTime;
        });
    }

    public void StopMove()
    {
        if (_moveStream == null)
        {
            return;
        }
        _moveStream.Dispose();
        _onStopMove.Invoke();
    }

    private void OnEnable()
    {
        if (!_moveOnEnable)
        {
            return;
        }

        Observable.TimerFrame(1).Subscribe(_ =>
        {
            StartMove();
        });
    }

    private void OnDisable()
    {
        StopMove();
    }
}
