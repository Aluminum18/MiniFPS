using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class IntervalAction : MonoBehaviour
{
    [SerializeField]
    private bool _startOnEnable = false;
    [SerializeField][Tooltip("-1 = Infinity")] 
    private int _limit = -1;
    [SerializeField]
    private float _interval;
    public float Interval
    {
        set
        {
            _interval = value;
        }
    }
    [SerializeField]
    private UnityEvent _action;

    private int _currentCount;
    private CompositeDisposable _cd = new CompositeDisposable();

    public void StartIntervalAction()
    {
        if (_interval <= 0f)
        {
            return;
        }

        _currentCount = _limit;
        if (_currentCount == 0)
        {
            return;
        }

        _cd.Clear();
        _action.Invoke();
        _currentCount--;
        Observable.Interval(System.TimeSpan.FromSeconds(_interval)).Subscribe(_ =>
        {
            if (_currentCount == 0)
            {
                StopIntervalAction();
                return;
            }

            _action.Invoke();
            _currentCount--;
        }).AddTo(_cd);
    }

    public void StopIntervalAction()
    {
        _cd.Clear();
    }

    private void OnEnable()
    {
        if (!_startOnEnable)
        {
            return;
        }
        StartIntervalAction();
    }

    private void OnDisable()
    {
        _cd.Clear();
    }
}
