using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRagDollActivate : MonoBehaviour
{
    [SerializeField]
    private List<Rigidbody> _rigidBodies;
    [SerializeField]
    private CharacterController _chaCon;
    [SerializeField]
    private Animator _animator;

    public void ActiveRagDoll(bool active)
    {
        _chaCon.enabled = !active;
        _animator.enabled = !active;

        ActiveColliderAndRb(active);

    }

    private void ActiveColliderAndRb(bool active)
    {
        for (int i = 0; i < _rigidBodies.Count; i++)
        {
            _rigidBodies[i].isKinematic = !active;
        }
    }
}
