using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterRagDollActivate : MonoBehaviour
{
    [SerializeField]
    private List<Rigidbody> _rigidBodies;
    [SerializeField]
    private CharacterController _chaCon;
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private UnityEvent _onActiveRagDoll;

    public void ActiveRagDoll(bool active)
    {
        if (active)
        {
            _onActiveRagDoll.Invoke();
        }

        _chaCon.enabled = !active;
        _animator.enabled = !active;

        ActiveColliderAndRb(active);

    }

    public void SwitchLayer(int layer)
    {
        for (int i = 0; i < _rigidBodies.Count; i++)
        {
            _rigidBodies[i].gameObject.layer = layer;
        }
    }

    private void ActiveColliderAndRb(bool active)
    {
        for (int i = 0; i < _rigidBodies.Count; i++)
        {
            _rigidBodies[i].isKinematic = !active;
        }
    }
}
