using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public static CubeManager Instance { get; private set; }

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

    public void ApplyExplosionForce(Rigidbody targetRb, Vector3 explosionOrigin)
    {
        if (targetRb != null)
        {
            Vector3 explosionDir = (targetRb.position - explosionOrigin).normalized;
            targetRb.AddForce(explosionDir * _explosionForce, ForceMode.Impulse);

            Vector3 randomTorque = new Vector3
            (
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f)
            );

            targetRb.AddTorque(randomTorque, ForceMode.Impulse);
        }
    }

    public void HandleCubeClick(GameObject cubeObject, Cube cubeScript)
    {
        bool shouldSplit = Random.Range(0f, 1f) <= cubeScript.SplitChance;

        if (shouldSplit && CanSplit(cubeScript))
        {
            SplitCube(cubeObject, cubeScript);
        }
        else
        {
            Rigidbody rb = cubeObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                ApplyExplosionForce(rb, cubeObject.transform.position);
            }
        }

        Destroy(cubeObject);
    }

    private bool CanSplit(Cube cubeScript)
    {
        return cubeScript.transform.localScale.x > 0.2f;
    }

    private void SplitCube(GameObject originalCube, Cube originalCubeScript)
    {
        int newCubeCount = Random.Range(2, 7);

        for (int i = 0; i < newCubeCount; i++)
        {
            CreateNewCube(originalCube, originalCubeScript);
        }
    }

    private void CreateNewCube(GameObject originalCube, Cube originalCubeScript)
    {
        GameObject newCube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        Vector3 randomOffset = Random.insideUnitSphere * 0.5f;
        newCube.transform.position = originalCube.transform.position + randomOffset;

        Vector3 newScale = originalCube.transform.localScale * 0.5f;
        newCube.transform.localScale = newScale;

        Rigidbody newRb = newCube.AddComponent<Rigidbody>();
        newRb.useGravity = true;
        newRb.mass = newScale.x;

        Cube newCubeScript = newCube.AddComponent<Cube>();
        newCubeScript.SplitChance = originalCubeScript.SplitChance * 0.5f;
        newCubeScript.OriginalScale = newScale;

        newCube.AddComponent<CubeClickHandler>();

        ApplyExplosionForce(newRb, originalCube.transform.position);
    }
}