using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletRay : MonoBehaviour
{
    [SerializeField]
    private Camera _uiCamera;
    [SerializeField]
    private Camera _mainCamera;
    [SerializeField]
    private Transform _crosshair;
    [SerializeField]
    private float _maxRayDistance = 500f;
    [SerializeField]
    private LayerMask _hitLayer;

    [SerializeField]
    private UnityEvent _onHitAnEnemy;

    public void CastBulletRay()
    {
        CrosshairToAimPoint();
    }

    private void CrosshairToAimPoint()
    {
        Vector3 screenPoint = _uiCamera.WorldToScreenPoint(_crosshair.position);
        Ray aimRay = _mainCamera.ScreenPointToRay(screenPoint);

        var hits = Physics.RaycastAll(aimRay, _maxRayDistance, _hitLayer);
        
        if (hits.Length == 0)
        {
            return;
        }

        bool isHitEnemy = false;

        for (int i = 0; i < hits.Length; i++)
        {
            var hit = hits[i];
            hit.collider.GetComponent<PhysicsEventBridge>()?.TriggerByCast(aimRay.direction);

            if (hit.collider.gameObject.layer == 6) // Enemy
            {
                isHitEnemy = true;
            }
        }

        if (isHitEnemy)
        {
            _onHitAnEnemy.Invoke();
        }
    }
}
