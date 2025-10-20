using UnityEngine;

public class CubeExploder : MonoBehaviour
{
    [SerializeField] private float _baseExplosionForce = 10f;
    [SerializeField] private float _explosionRadiusMultiplier = 2f;
    [SerializeField] private float _forceMultiplier = 1.5f;

    public void ApplyExplosionToCube(Cube cube, Vector3 explosionOrigin)
    {
        if (cube.Rigidbody == null)
        {
            cube.gameObject.AddComponent<Rigidbody>();
        }

        cube.Rigidbody.useGravity = true;
        cube.Rigidbody.mass = cube.transform.localScale.x;

        ApplyExplosionForce(cube.Rigidbody, explosionOrigin, cube.OriginalScale.x);
    }

    public void CreateExplosion(Vector3 explosionOrigin, float baseRadius, float baseForce, LayerMask cubeLayerMask)
    {
        float explosionRadius = baseRadius * _explosionRadiusMultiplier;

        Collider[] colliders = Physics.OverlapSphere(explosionOrigin, explosionRadius, cubeLayerMask);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Cube cube) && cube.Rigidbody != null)
            {
                ApplyExplosionToCubeAtDistance(cube, explosionOrigin, baseForce, explosionRadius);
            }
        }
    }

    private void ApplyExplosionForce(Rigidbody targetRb, Vector3 explosionOrigin, float cubeSize)
    {
        if (targetRb != null)
        {
            Vector3 explosionDir = (targetRb.position - explosionOrigin).normalized;
            float force = _baseExplosionForce * cubeSize * _forceMultiplier;
            targetRb.AddForce(explosionDir * force, ForceMode.Impulse);

            Vector3 randomTorque = new Vector3(
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f)
            );

            targetRb.AddTorque(randomTorque, ForceMode.Impulse);
        }
    }

    private void ApplyExplosionToCubeAtDistance(Cube cube, Vector3 explosionOrigin, float baseForce, float explosionRadius)
    {
        Rigidbody rb = cube.Rigidbody;
        if (rb == null) return;

        Vector3 explosionDir = (rb.position - explosionOrigin);
        float distance = explosionDir.magnitude;

        explosionDir = explosionDir.normalized;

        float distanceFactor = 1f - Mathf.Clamp01(distance / explosionRadius);
        float sizeFactor = cube.OriginalScale.x;

        float finalForce = baseForce * distanceFactor * sizeFactor * _forceMultiplier;

        rb.AddForce(explosionDir * finalForce, ForceMode.Impulse);

        Vector3 randomTorque = new Vector3(
            Random.Range(-10f, 10f),
            Random.Range(-10f, 10f),
            Random.Range(-10f, 10f)
        );
        rb.AddTorque(randomTorque, ForceMode.Impulse);
    }
}