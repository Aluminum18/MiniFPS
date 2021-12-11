using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private Vector3Variable _giveUpPoint;

    [SerializeField]
    private UnityEvent _onStartMove;
    [SerializeField]
    private UnityEvent _onStopMove;

    [Header("Config")]
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _rotateSpeed;
    [SerializeField]
    private Vector3 _finishLineDirection;
    [SerializeField]
    private CharacterController _charCon;
    [SerializeField]
    private Transform _characterRotateTransform;
    [SerializeField]
    private Animator _animator;

    [Header("Inpsec")]
    [SerializeField]
    private float _currentSpeed;
    [SerializeField]
    private Vector3 _direction;

    private IDisposable _rotateStream;
    private IDisposable _moveStream;

    public void StartMoveForward()
    {
        SetSpeed(_speed);
        MoveToDes();
        RotateToDirection();
        _onStartMove.Invoke();
    }

    public void StopMove()
    {
        if (_moveStream == null)
        {
            return;
        }

        _moveStream.Dispose();
        _onStopMove.Invoke();

        SetSpeed(0f);
    }

    private LTDescr _walkSpeedAnimTween;
    public void SetSpeed(float speed)
    {
        if (_walkSpeedAnimTween != null)
        {
            LeanTween.cancel(_walkSpeedAnimTween.id);
        }

        _animator.SetFloat("MoveSpeed", speed);

        _currentSpeed = speed;
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }

    public void HeadToFinishLine()
    {
        _direction = _finishLineDirection;
    }

    public void HeadToGiveUpPoint()
    {
        _direction = _giveUpPoint.Value - _characterRotateTransform.position;
    }

    public void MultiplySpeedBy(float ratio)
    {
        SetSpeed(_currentSpeed * ratio);
    }

    private void MoveToDes()
    {
        if (_moveStream != null)
        {
            _moveStream.Dispose();
        }

        _moveStream = Observable.EveryUpdate().Subscribe(_ =>
        {
            if (!_charCon.enabled)
            {
                StopMove();
                return;
            }

            _charCon.SimpleMove(_characterRotateTransform.forward * _currentSpeed);
        });
    }

    private void RotateToDirection()
    {
        if (_rotateStream != null)
        {
            _rotateStream.Dispose();
        }

        Quaternion rotateTo;
        _rotateStream = Observable.EveryUpdate().Subscribe(_ =>
        {
            rotateTo = Quaternion.LookRotation(_direction);

            if (Quaternion.Angle(_characterRotateTransform.rotation, rotateTo) < 1f)
            {
                _characterRotateTransform.rotation = rotateTo;
                return;
            }

            _characterRotateTransform.rotation = Quaternion.RotateTowards(_characterRotateTransform.rotation, rotateTo, _rotateSpeed * Time.deltaTime);
        });
    }

    private void OnDisable()
    {
        StopMove();
        _rotateStream?.Dispose();

        if (_walkSpeedAnimTween == null)
        {
            return;
        }

        LeanTween.cancel(_walkSpeedAnimTween.id);
    }
}
