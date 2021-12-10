using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ContactJointTo : MonoBehaviour
{
    [SerializeField]
    private LayerMask _contactMask;
    [SerializeField]
    private Rigidbody _rigidBody;

    [SerializeField]
    private UnityEvent _onRemovedJoint;

    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer & _contactMask) == 0)
        {
            return;
        }

        var joint = _rigidBody.gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = other.attachedRigidbody;
    }

    private void OnDisable()
    {
        var fixedJoint = GetComponent<FixedJoint>();
        if (fixedJoint == null)
        {
            return;
        }

        Destroy(fixedJoint);

        _onRemovedJoint.Invoke();
    }
}
