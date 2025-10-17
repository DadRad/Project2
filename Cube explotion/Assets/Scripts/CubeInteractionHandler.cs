using UnityEngine;
using System.Collections.Generic;

public class CubeInteractionHandler : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Raycaster _raycaster;
    [SerializeField] private CubeExploder _cubeExploder;
    [SerializeField] private CubeSpawner _cubeSpawner;

    private void Start()
    {
        _inputReader.OnClickPerformed += HandleClick;
        _raycaster.OnCubeHit += HandleCubeInteraction;
    }

    private void OnDestroy()
    {
        _inputReader.OnClickPerformed -= HandleClick;
        _raycaster.OnCubeHit -= HandleCubeInteraction;
    }

    private void HandleClick(Vector2 screenPosition)
    {
        _raycaster.PerformRaycast(screenPosition);
    }

    private void HandleCubeInteraction(Cube cube)
    {
        bool shouldSplit = Random.Range(0f, 1f) <= cube.SplitChance;
        Vector3 interactionPoint = cube.transform.position;

        if (shouldSplit)
        {
            int minRandomValue = 2;
            int maxRandomValue = 7;
            int newCubeCount = Random.Range(minRandomValue, maxRandomValue + 1);

            List<Cube> newCubes = _cubeSpawner.SpawnMultipleCubes(cube, interactionPoint, newCubeCount);

            Destroy(cube.gameObject);

            foreach (Cube newCube in newCubes)
            {
                _cubeExploder.ApplyExplosionToCube(newCube, interactionPoint);
            }
        }

        Destroy(cube.gameObject);
    }
}