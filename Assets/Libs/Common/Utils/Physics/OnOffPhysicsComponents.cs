using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffPhysicsComponents : MonoBehaviour
{
    [SerializeField]
    private List<Collider> _colliders;
    [SerializeField]
    private List<Rigidbody> _rigidBodies;

    public void ActiveColliders(bool active)
    {
        for (int i = 0; i < _colliders.Count; i++)
        {
            _colliders[i].enabled = active;
        }
    }

    public void SetKinematic(bool isKinematic)
    {
        for (int i = 0; i < _rigidBodies.Count; i++)
        {
            _rigidBodies[i].isKinematic = isKinematic;
        }
    }
}
