using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class RandomDirectionMove : MonoBehaviour
{
    [SerializeField]
    private bool _moveOnEnable = true;
    [SerializeField]
    private float _moveDelay = 0f;
    [SerializeField]
    private Transform _movedTransform;
    [SerializeField]
    private Transform _point1;
    [SerializeField]
    private Transform _point2;
    [SerializeField]
    private LeanTweenType _easeType;
    [SerializeField]
    private float _moveTime;

    [SerializeField]
    private UnityEvent _onStartMove;
    [SerializeField]
    private UnityEvent _onFinishMove;

    private void OnEnable()
    {
        if (!_moveOnEnable)
        {
            return;
        }

        if (_moveDelay > 0f)
        {
            MoveWithDelay();
            return;
        }

        // delay 1 frame for waiting transform setup
        Observable.TimerFrame(1).Subscribe(_ =>
        {
            Move();
        });
    }
    Vector3 _random = Vector3.zero;
    public void Move()
    {
        Vector3 randomPoint = Vector3.Lerp(_point1.position, _point2.position, Random.Range(0f, 1f));

        _random = randomPoint;

        _onStartMove.Invoke();
        LeanTween.move(_movedTransform.gameObject, randomPoint, _moveTime).setEase(_easeType);

        Observable.Timer(System.TimeSpan.FromSeconds(_moveTime)).Subscribe(_ =>
        {
            _onFinishMove.Invoke();
        });
    }

    public void MoveWithDelay()
    {
        Observable.Timer(System.TimeSpan.FromSeconds(_moveDelay)).Subscribe(_ =>
        {
            Move();
        });
    }

    private void OnDrawGizmos()
    {
        if (_movedTransform == null
            || _point1 == null
            || _point2 == null)
        {
            return;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawLine(_movedTransform.position, _point1.position);
        Gizmos.DrawLine(_movedTransform.position, _point2.position);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(_movedTransform.position, _random);

    }

}
