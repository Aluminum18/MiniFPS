using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;

public class SequentialAction : MonoBehaviour
{
    [SerializeField]
    private bool _startOnEnable = false;

    [SerializeField]
    private List<ActionInSequence> _actions;

    private CompositeDisposable _cd = new CompositeDisposable();

    public void StartActions()
    {
        _cd.Clear();

        float delay = 0f;
        for (int i = 0; i < _actions.Count; i++)
        {
            var actionInfo = _actions[i];

            Observable.Timer(System.TimeSpan.FromSeconds(delay)).Subscribe(_ =>
            {
                if (actionInfo.action == null)
                {
                    return;
                }

                actionInfo.action.Invoke();
            }).AddTo(_cd);

            delay += actionInfo.timeToNextAction;

        }
    }

    private void OnEnable()
    {
        if (_startOnEnable)
        {
            StartActions();
        }
    }

    private void OnDestroy()
    {
        _cd.Clear();
    }
}

[System.Serializable]
public class ActionInSequence
{
    public UnityEvent action;
    public float timeToNextAction;
}
