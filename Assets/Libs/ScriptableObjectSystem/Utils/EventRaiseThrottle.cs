using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class EventRaiseThrottle : MonoBehaviour
{
    [SerializeField]
    private GameEvent _eventIn;
    [SerializeField]
    private List<GameEvent> _eventsOut;
    [SerializeField]
    private float _throttle;
    [Header("Inspec")]
    [SerializeField]
    private float _timeUntilNextEvent;

    public void SetThrottle(float throttle)
    {
        _throttle = throttle;
    }

    private CompositeDisposable _cd = new CompositeDisposable();
    private void OnEnable()
    {
        _eventIn.Subcribe(RaiseEvent);
    }

    private void OnDisable()
    {
        _cd.Clear();
        _eventIn.Unsubcribe(RaiseEvent);
    }

    private void RaiseEvent(params object[] args)
    {
        if (_timeUntilNextEvent > 0f)
        {
            return;
        }

        _timeUntilNextEvent = _throttle;
        Observable.EveryUpdate().Subscribe(_ =>
        {
            if (_timeUntilNextEvent <= 0f)
            {
                _cd.Clear();
                return;
            }

            _timeUntilNextEvent -= Time.deltaTime;
        }).AddTo(_cd);

        for (int i = 0; i < _eventsOut.Count; i++)
        {
            _eventsOut[i].Raise();
        }
    }
}
