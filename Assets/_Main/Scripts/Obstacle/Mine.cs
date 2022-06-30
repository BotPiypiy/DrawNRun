using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float radius;
    [SerializeField] private float force;
    private bool isActive;

    private void Awake()
    {
        isActive = true;
    }

    public void Explosion()
    {
        if (isActive)
        {
            isActive = false;

            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, playerLayer, QueryTriggerInteraction.Ignore);
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].GetComponent<CharacterBehaviour>().Remove();
                colliders[i].attachedRigidbody.AddExplosionForce(force, transform.position, radius, 1, ForceMode.Impulse);
            }
        }
    }
}
