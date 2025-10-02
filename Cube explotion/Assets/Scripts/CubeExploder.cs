using UnityEngine;

public class CubeExploder : MonoBehaviour
{
    public static CubeExploder Instance { get; private set; }

    [SerializeField] private float _explosionForce = 10f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ApplyExplosionToCube(Cube cube, Vector3 explosionOrigin)
    {
        Rigidbody rb = cube.GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = cube.gameObject.AddComponent<Rigidbody>();
        }

        rb.useGravity = true;
        rb.mass = cube.transform.localScale.x;

        ApplyExplosionForce(rb, explosionOrigin);
    }

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
}