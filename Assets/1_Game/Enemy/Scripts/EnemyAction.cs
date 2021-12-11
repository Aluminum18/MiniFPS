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
    private UnityEvent _onDefeated;

    [Header("Config")]
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
        _onEnable.Invoke();
    }
}
