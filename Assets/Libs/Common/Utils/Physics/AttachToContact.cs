using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToContact : MonoBehaviour
{
    [SerializeField]
    private LayerMask _jointMask;

    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer & _jointMask) == 0)
        {
            return;
        }
        transform.parent = other.transform;
    }
}
