using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private StringVariable _requestLoadScene;

    [SerializeField]
    private string _requestSceneName;
    [SerializeField]
    private float _loadDelay = 1f;

    [SerializeField]
    private UnityEvent _onReceivedLoadRequest;
    [SerializeField]
    private UnityEvent _onStartLoad;

    public void LoadRequestedScene()
    {
        LoadRequestedScene(_requestSceneName);
    }

    public void LoadRequestedScene(string sceneName)
    {
        _onReceivedLoadRequest.Invoke();
        Observable.Timer(System.TimeSpan.FromSeconds(_loadDelay)).Subscribe(_ =>
        {
            _onStartLoad.Invoke();
            _requestLoadScene.Value = sceneName;

            SceneManager.LoadScene("BufferScene");
        });
    }

    public void BufferSceneOnly_LoadScene()
    {
        SceneManager.LoadScene(_requestLoadScene.Value);
    }
}
