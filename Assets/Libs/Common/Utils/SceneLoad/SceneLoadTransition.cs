using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoadTransition : MonoSingleton<SceneLoadTransition>
{
    [SerializeField]
    private UnityEvent _onEnable;
    [SerializeField]
    private UnityEvent _onSceneLoaded;

    private void OnEnable()
    {
        _onEnable.Invoke();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene loadedScene, LoadSceneMode loadMode)
    {
        if (loadedScene.name == "BufferScene")
        {
            return;
        }

        Observable.TimerFrame(2).Subscribe(_ =>
        {
            _onSceneLoaded.Invoke();
        });
    }
}
