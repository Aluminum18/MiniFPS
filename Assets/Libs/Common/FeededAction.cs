using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class FeededAction : MonoBehaviour
{
    [SerializeField]
    private bool _startOnEnable = false;
    [SerializeField]
    private float _aliveTimeAfterFeed = 0.2f;

    [SerializeField]
    private UnityEvent _initAction;
    [SerializeField]
    private UnityEvent _onFeededAction;
    [SerializeField]
    private UnityEvent _tempDisableAction;

    [Header("Inspec")]
    [SerializeField]
    private float _tempDisableAfter;

    private bool _tempDisable;

    private CompositeDisposable _cd = new CompositeDisposable();

    private bool _initFeed = true;
    
    public void Feed()
    {
        if (_initFeed)
        {
            InitFeed();
            _initFeed = false;
        }

        _tempDisableAfter = _aliveTimeAfterFeed;

        _tempDisable = false;

        _onFeededAction.Invoke();
    }

    public void PermanentDisableAction()
    {
        _cd.Clear();
        _initFeed = true;
    }

    private void InitFeed()
    {
        TrackLifeTime();
        _initAction.Invoke();
    }

    private void TrackLifeTime()
    {
        Observable.EveryUpdate().Subscribe(_ =>
        {
            if (_tempDisableAfter <= 0f && !_tempDisable)
            {
                _tempDisable = true;
                _tempDisableAction.Invoke();               
            }

            _tempDisableAfter -= Time.deltaTime;
        }).AddTo(_cd);
    }

    private void OnEnable()
    {
        _initFeed = true;
        if (_startOnEnable)
        {
            Feed();
        }
    }

    private void OnDisable()
    {
        PermanentDisableAction();
    }
}
