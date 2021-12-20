using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsEventBridge : MonoBehaviour
{
    [SerializeField]
    private LayerMask _contactMask;
    [SerializeField]
    private UnityEvent _onTriggerEnter;
    [SerializeField]
    private UnityGameObjectEvent _onTriggerEnterT;
    [SerializeField]
    private Unity2GameObjectsEvent _onTriggerEnterT1T2;
    [SerializeField]
    private UnityRaycastEvent _onRayCastTrigger;
    [SerializeField]
    private Unity3Vector3Event _onRayCastTrigger2;

    [Header("Inspec")]
    [SerializeField]
    private GameObject _recentlyContact;
    public GameObject RecentlyContact => _recentlyContact;

    public void TriggerByCast(Vector3 castDirection)
    {
        _onRayCastTrigger.Invoke(castDirection, GetComponent<Collider>());
    }

    public void TriggerByCast2(Vector3 hitPosition)
    {
        _onRayCastTrigger2.Invoke(hitPosition);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if ((1 << collision.gameObject.layer & _contactMask) == 0)
        {
            return;
        }

        _onTriggerEnter.Invoke();
        _onTriggerEnterT.Invoke(collision.gameObject);
        _onTriggerEnterT1T2.Invoke(collision.gameObject, gameObject);
        _recentlyContact = collision.gameObject;
    }
}
