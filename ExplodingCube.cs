using UnityEngine;
using System.Collections;

public class ExplodingCube : MonoBehaviour
{
    [SerializeField] private float _explosionForce = 10f;

    private Rigidbody _rb;
    private Renderer _cubeRenderer;
    private float _splitChance = 1f;
    private Vector3 _originalScale;
    private bool _hasSplit = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _cubeRenderer = GetComponent<Renderer>();

        SetRandomColor();

        if (_rb != null)
        {
            _rb.useGravity = true;
            _rb.mass = transform.localScale.x;
        }

        _originalScale = transform.localScale;
    }

    void OnMouseDown()
    {
        if (_hasSplit) return;

        bool shouldSplit = Random.Range(0f, 1f) <= _splitChance;

        if (shouldSplit)
        {
            SplitCube();
        }

        Destroy(gameObject);
    }

    void SplitCube()
    {
        _hasSplit = true;

        int newCubeCount = Random.Range(2, 7);

        for (int i = 0; i < newCubeCount; i++)
        {
            CreateNewCube();
        }
    }

    void CreateNewCube()
    {
        GameObject newCube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        Vector3 randomOffset = Random.insideUnitSphere * 0.5f;
        newCube.transform.position = transform.position + randomOffset;

        newCube.transform.localScale = _originalScale * 0.5f;

        Rigidbody newRb = newCube.AddComponent<Rigidbody>();
        newCube.AddComponent<ExplodingCube>();

        ExplodingCube newCubeScript = newCube.GetComponent<ExplodingCube>();
        newCubeScript._splitChance = _splitChance * 0.5f;
        newCubeScript._originalScale = newCube.transform.localScale;

        ApplyExplosionForce(newRb);
    }

    void ApplyExplosionForce(Rigidbody targetRb)
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

    void SetRandomColor()
    {
        if (_cubeRenderer != null)
        {
            _cubeRenderer.material.color = new Color(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f)
            );
        }
    }
}