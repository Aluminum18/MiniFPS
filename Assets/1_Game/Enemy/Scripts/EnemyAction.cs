using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAction : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _onEnable;
    [SerializeField]
    private UnityEvent _onStartedGame;
    [SerializeField]
    private UnityEvent _onHeardStatueCommand;
    [SerializeField]
    private UnityEvent _onDidStatue;
    [SerializeField]
    private UnityEvent _onFailedToDoStatue;
    [SerializeField]
    private UnityEvent _onCanceledStatue;
    [SerializeField]
    private UnityEvent _onDefeated;

    [Header("Config")]
    [SerializeField][Range(0f, 1f)]
    private float _failedToDoStatueRatio;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private ObjectSpawner _bloodSpawner;
    [SerializeField]
    private float _defeatedForceAmplify = 2f;

    [Header("Inspec")]
    [SerializeField]
    private bool _hittable = true;
    public bool Hittable
    {
        set
        {
            _hittable = value;
        }
    }

    public void StartPlayGame()
    {
        _onStartedGame.Invoke();
    }

    public void DoStatue()
    {
        _onHeardStatueCommand.Invoke();
        float roll = UnityEngine.Random.Range(0f, 1f);
        if  (roll < _failedToDoStatueRatio)
        {
            FailedToDoStatue();
            return;
        }
        _animator.speed = 0f;
        _onDidStatue.Invoke();
    }

    public void CancelStatue()
    {
        _animator.speed = 1f;
        _onCanceledStatue.Invoke();
    }

    public void FailedToDoStatue()
    {
        int roll = UnityEngine.Random.Range(1, 4);

        _animator.Play("IdleDynamic " + roll.ToString());

        _onFailedToDoStatue.Invoke();
    }

    public void Defeated(Vector3 bulletDirection, Collider bodyPart)
    {
        if (!_hittable)
        {
            return;
        }

        _hittable = false;

        _onDefeated.Invoke();

        Observable.TimerFrame(1).Subscribe(_ =>
        {
            var rb = bodyPart.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.Normalize(bulletDirection) * _defeatedForceAmplify, ForceMode.Impulse);

            var blood = _bloodSpawner.SpawnAndReturnObject();
            blood.transform.position = rb.position;
        });


    }

    public void ForceDefeat()
    {
        _onDefeated.Invoke();
    }

    private void SpawnBlood(GameObject bullet)
    {
        var bloodFx = _bloodSpawner.SpawnAndReturnObject();
        bloodFx.transform.position = bullet.transform.position;
    }

    private void OnEnable()
    {
        _hittable = true;
        _animator.speed = 1f;
        _onEnable.Invoke();
    }
}
