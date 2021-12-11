using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class Referee : MonoBehaviour
{
    [Header("Reference - Read")]
    [SerializeField]
    private FloatVariable _timeToNextStatue;
    [SerializeField]
    private FloatVariable _statuePeriod;

    [Header("Reference - Write")]
    [SerializeField]
    private FloatVariable _remainStatueTime;

    [Header("Events in")]
    [SerializeField]
    private GameEvent _onEnemyDefeated;

    [SerializeField]
    private UnityEvent _onGoodEliminated;
    [SerializeField]
    private UnityEvent _onBadEliminated;
    [SerializeField]
    private UnityEvent _onRaisedStatueCommand;
    [SerializeField]
    private UnityEvent _onEndStatuePeriod;

    [Header("Config")]
    [SerializeField]
    private float _maxDelayStatueCommand = 4f;

    private IDisposable _countStatueTimeStream;

    public void StartTrackNextStatue()
    {
        Observable.Timer(TimeSpan.FromSeconds(_timeToNextStatue.Value)).Subscribe(_ =>
        {
            RaiseStatueCommand();
        });
    }

    public void RaiseStatueCommand()
    {
        Observable.Timer(TimeSpan.FromSeconds(UnityEngine.Random.Range(0f, 4f))).Subscribe(_ =>
        {
            CountStatuePeriod();
            _onRaisedStatueCommand.Invoke();
        });
    }

    public void EndStatuePeriod()
    {
        _onEndStatuePeriod.Invoke();
    }

    private void CountStatuePeriod()
    {
        if (_countStatueTimeStream != null)
        {
            _countStatueTimeStream.Dispose();
        }

        _remainStatueTime.Value = _statuePeriod.Value;

        _countStatueTimeStream = Observable.EveryUpdate().Subscribe(_ =>
        {
            if (_remainStatueTime.Value <= 0f)
            {
                _remainStatueTime.Value = 0f;
                EndStatuePeriod();
                _countStatueTimeStream.Dispose();
                return;
            }

            _remainStatueTime.Value -= Time.deltaTime;
        });
    }

    private void ValidateDefeatedEnemy(object[] args)
    {
        var status = (EnemyRuleComplianceStatus)args[0];

        if (EnemyRuleComplianceStatus.Good.Equals(status))
        {
            _onBadEliminated.Invoke();
            return;
        }

        _onGoodEliminated.Invoke();
    }

    private void OnEnable()
    {
        _onEnemyDefeated.Subcribe(ValidateDefeatedEnemy);
    }

    private void OnDisable()
    {
        _onEnemyDefeated.Unsubcribe(ValidateDefeatedEnemy);
    }
}
