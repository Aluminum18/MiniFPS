using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPhysics : MonoBehaviour
{
    [SerializeField]
    private List<Rigidbody> _targetRigidBodies;

    private void OnDisable()
    {
        for (int i = 0; i < _targetRigidBodies.Count; i++)
        {
            var rb = _targetRigidBodies[i];
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
