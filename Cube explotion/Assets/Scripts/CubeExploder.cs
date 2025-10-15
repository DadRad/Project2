using UnityEngine;

public class CubeExploder : MonoBehaviour
{
    [SerializeField] private float _explosionForce = 10f;

    private void ApplyExplosionForce(Rigidbody targetRb, Vector3 explosionOrigin)
    {
        if (targetRb != null)
        {
            Vector3 explosionDir = (targetRb.position - explosionOrigin).normalized;
            targetRb.AddForce(explosionDir * _explosionForce, ForceMode.Impulse);

            Vector3 randomTorque = new Vector3(
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f)
            );

            targetRb.AddTorque(randomTorque, ForceMode.Impulse);
        }
    }

    public void ApplyExplosionToCube(Cube cube, Vector3 explosionOrigin)
    {
        if (cube.Rigidbody == null)
        {
            cube.gameObject.AddComponent<Rigidbody>();
        }

        cube.Rigidbody.useGravity = true;
        cube.Rigidbody.mass = cube.transform.localScale.x;

        ApplyExplosionForce(cube.Rigidbody, explosionOrigin);
    }
}