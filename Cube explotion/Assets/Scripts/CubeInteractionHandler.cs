using UnityEngine;
using System.Collections.Generic;

public class CubeInteractionHandler : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Raycaster _raycaster;
    [SerializeField] private CubeExploder _cubeExploder;
    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private LayerMask _cubeLayerMask;
    [SerializeField] private float _baseExplosionRadius = 3f;
    [SerializeField] private float _baseExplosionForce = 15f;

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
        else
        {
            float cubeSize = cube.OriginalScale.x;
            float explosionRadius = _baseExplosionRadius * (1f / cubeSize);
            float explosionForce = _baseExplosionForce * (1f / cubeSize);

            _cubeExploder.CreateExplosion(interactionPoint, explosionRadius, explosionForce, _cubeLayerMask);

            Destroy(cube.gameObject);
        }

        Destroy(cube.gameObject);
    }
}