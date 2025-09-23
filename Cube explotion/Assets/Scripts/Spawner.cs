using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Exploder))]
public class Spawner : MonoBehaviour
{
    private Rigidbody _rb;
    private bool _hasSplit = false;
    private Exploder _exploder;
    private Cube _cube;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _exploder = GetComponent<Exploder>();
        _cube = GetComponent<Cube>();

        if (_rb != null)
        {
            _rb.useGravity = true;
            _rb.mass = transform.localScale.x;
        }

        _cube.OriginalScale = transform.localScale;
    }

    private void OnMouseDown()
    {
        if (_hasSplit) return;

        bool shouldSplit = Random.Range(0f, 1f) <= _cube.SplitChance;

        if (shouldSplit)
        {
            SplitCube();
        }
        else
        {
            _exploder.ApplyExplosionForce(_rb);
        }

        Destroy(gameObject);
    }

    private void SplitCube()
    {
        _hasSplit = true;

        int newCubeCount = Random.Range(2, 7);

        for (int i = 0; i < newCubeCount; i++)
        {
            CreateNewCube();
        }
    }

    private void CreateNewCube()
    {
        GameObject newCube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        Vector3 randomOffset = Random.insideUnitSphere * 0.5f;
        newCube.transform.position = transform.position + randomOffset;
        newCube.transform.localScale = _cube.OriginalScale * 0.5f;

        Rigidbody newRb = newCube.AddComponent<Rigidbody>();
        newCube.AddComponent<Exploder>();
        newCube.AddComponent<Cube>();
        newCube.AddComponent<Spawner>();

        Cube newCubeScript = newCube.GetComponent<Cube>();
        newCubeScript.SplitChance = _cube.SplitChance * 0.5f;
        newCubeScript.OriginalScale = newCube.transform.localScale;

        _exploder.ApplyExplosionForce(newRb);
    }
}