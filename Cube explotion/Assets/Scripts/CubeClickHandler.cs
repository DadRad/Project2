using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cube))]
public class CubeClickHandler : MonoBehaviour
{
    private Cube _cube;

    private void Start()
    {
        _cube = GetComponent<Cube>();
    }

    private void OnMouseDown()
    {
        if (_cube != null)
        {
            CubeManager.Instance.HandleCubeClick(gameObject, _cube);
        }
    }
}