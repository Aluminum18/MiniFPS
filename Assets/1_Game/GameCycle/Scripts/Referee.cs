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
    [SerializeField]
    private IntegerVariable _remainEnemies;
    [SerializeField]
    private FloatVariable _accuracy;
    [SerializeField]
    private FloatVariable _goodShot;
    [SerializeField]
    private FloatVariable _detection;

    [Header("Reference - Write")]
    [SerializeField]
    private FloatVariable _remainStatueTime;
    [SerializeField]
    private StringVariable _finalResult;

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
    [SerializeField]
    private UnityEvent _onEndedGame;

    [Header("Config")]
    [SerializeField]
    private float _maxDelayStatueCommand = 4f;

    private bool _isGameEnded = false;

    private IDisposable _countStatueTimeStream;

    public void StartTrackNextStatue()
    {
        Observable.Timer(TimeSpan.FromSeconds(_timeToNextStatue.Value)).Subscribe(_ =>
        {
            if (_isGameEnded)
            {
                return;
            }

            RaiseStatueCommand();
        });
    }

    public void RaiseStatueCommand()
    {
        Observable.Timer(TimeSpan.FromSeconds(UnityEngine.Random.Range(0f, 4f))).Subscribe(_ =>
        {
            if (_isGameEnded)
            {
                return;
            }

            CountStatuePeriod();
            _onRaisedStatueCommand.Invoke();
        });
    }

    public void EndStatuePeriod()
    {
        _onEndStatuePeriod.Invoke();
    }

    public void EndGame()
    {
        _isGameEnded = true;
        _countStatueTimeStream?.Dispose();
        _onEndedGame.Invoke();
    }

    public void JudgeResult()
    {
        Observable.Timer(TimeSpan.FromSeconds(1.2f)).Subscribe(_ =>
        {
            if (_accuracy.Value < 0.8f
                || _goodShot.Value < 0.8f
                || _detection.Value < 0.5f)
            {
                _finalResult.Value = "You've been fired!";
                return;
            }

            _finalResult.Value = "Good job!";
        });

    }

    private void CountStatuePeriod()
    {
        _countStatueTimeStream?.Dispose();

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

    private void TrackRemainEnemies(int remainEnemies)
    {
        if (remainEnemies > 0)
        {
            return;
        }

        EndGame();
    }

    private void OnEnable()
    {
        _onEnemyDefeated.Subcribe(ValidateDefeatedEnemy);
        _remainEnemies.OnValueChange += TrackRemainEnemies;
    }

    private void OnDisable()
    {
        _onEnemyDefeated.Unsubcribe(ValidateDefeatedEnemy);
        _remainEnemies.OnValueChange -= TrackRemainEnemies;

        _countStatueTimeStream?.Dispose();
    }
}
