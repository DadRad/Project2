using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    [SerializeField] private float _explosionForce = 10f;

    public void ApplyExplosionForce(Rigidbody targetRb)
    {
        if (targetRb != null)
        {
            Vector3 explosionDir = (targetRb.position - transform.position).normalized;

            targetRb.AddForce(explosionDir * _explosionForce, ForceMode.Impulse);

            Vector3 randomTorque = new Vector3(
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f)
            );

            targetRb.AddTorque(randomTorque, ForceMode.Impulse);
        }
    }
}
