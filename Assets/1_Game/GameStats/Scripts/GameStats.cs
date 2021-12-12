using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStats : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _onEnable;

    private void OnEnable()
    {
        _onEnable.Invoke();
    }
}
