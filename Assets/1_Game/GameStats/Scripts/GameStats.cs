using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStats : MonoBehaviour
{
    [Header("Reference - Read")]
    [SerializeField]
    private IntegerVariable _shotCount;
    [SerializeField]
    private IntegerVariable _goodShotCount;
    [SerializeField]
    private IntegerVariable _badShotCount;
    [SerializeField]
    private IntegerVariable _violatedEnemyCount;
    [SerializeField]
    private IntegerVariable _missDetectCount;
    [SerializeField]
    private IntegerVariable _currentStatueMissDetectCount;

    [Header("Reference - Write")]
    [SerializeField]
    private FloatVariable _accuracy;
    [SerializeField]
    private FloatVariable _goodShot;
    [SerializeField]
    private FloatVariable _detection;
    [SerializeField]
    private IntegerVariable _lastStatueMissDetectCount;

    [SerializeField]
    private UnityEvent _onEnable;

    private void OnEnable()
    {
        _onEnable.Invoke();
    }

    public void CalculateGameStats()
    {
        _detection.Value = 1f - (float)(_missDetectCount.Value) / _violatedEnemyCount.Value;

        if (_shotCount.Value == 0)
        {
            _accuracy.Value = 0f;
            _goodShot.Value = 0f;
            return;
        }

        _accuracy.Value = (float)(_goodShotCount.Value + _badShotCount.Value) / _shotCount.Value;
        _goodShot.Value = (float)_goodShotCount.Value / (_goodShotCount.Value + _badShotCount.Value);
    }

    public void RecordLastStatueMissDetectCount()
    {
        _lastStatueMissDetectCount.Value = _currentStatueMissDetectCount.Value;
        _currentStatueMissDetectCount.Value = 0;
    }
}
