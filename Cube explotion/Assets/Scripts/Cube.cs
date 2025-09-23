using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour
{
    private Renderer _cubeRenderer;

    public Vector3 OriginalScale;
    public float SplitChance = 1f;

    private void Start()
    {
        _cubeRenderer = GetComponent<Renderer>();
        SetRandomColor();
    }

    private void SetRandomColor()
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