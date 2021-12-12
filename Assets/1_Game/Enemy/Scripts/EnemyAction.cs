using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAction : MonoBehaviour
{
    [Header("Events out")]
    [SerializeField]
    private GameEvent _onEnemyDefeated;

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
    [SerializeField]
    private UnityEvent _onDisable;

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
    [SerializeField]
    private bool _isPlaying = false;
    [SerializeField]
    private EnemyRuleComplianceStatus _complianceStatus;

    public void StartPlayGame()
    {
        _onStartedGame.Invoke();
    }

    public void DoStatue()
    {
        _onHeardStatueCommand.Invoke();

        if (!_isPlaying)
        {
            return;
        }

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
        _complianceStatus = EnemyRuleComplianceStatus.Good;
        _animator.speed = 1f;
        _onCanceledStatue.Invoke();
    }

    public void FailedToDoStatue()
    {
        _complianceStatus = EnemyRuleComplianceStatus.Violated;

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
        _isPlaying = false;

        _onDefeated.Invoke();
        _onEnemyDefeated.Raise(_complianceStatus);

        Observable.TimerFrame(1).Subscribe(_ =>
        {
            var rb = bodyPart.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.Normalize(bulletDirection) * _defeatedForceAmplify, ForceMode.Impulse);

            var blood = _bloodSpawner.SpawnAndReturnObject();
            blood.transform.position = rb.transform.position;
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
        _complianceStatus = EnemyRuleComplianceStatus.Good;

        _hittable = true;
        _isPlaying = true;

        _animator.speed = 1f;
        _onEnable.Invoke();
    }

    private void OnDisable()
    {
        _onDisable.Invoke();
    }
}

public enum EnemyRuleComplianceStatus
{
    Good = 0,
    Violated = 1
}
