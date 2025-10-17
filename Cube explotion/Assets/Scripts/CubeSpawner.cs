using UnityEngine;
using System.Collections.Generic;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;

    public Cube SpawnCube(Vector3 position, Vector3 scale, float splitChance)
    {
        Cube cube = Instantiate(_cubePrefab, position, Quaternion.identity);
        cube.name = "Cube";
        cube.Initialize(splitChance, scale);

        return cube;
    }

    public List<Cube> SpawnMultipleCubes(Cube originalCube, Vector3 explosionOrigin, int count)
    {
        List<Cube> newCubes = new List<Cube>();

        for (int i = 0; i < count; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * 0.5f;
            Vector3 newScale = originalCube.OriginalScale * 0.5f;
            float newSplitChance = originalCube.SplitChance * 0.5f;

            Cube newCube = SpawnCube(explosionOrigin + randomOffset, newScale, newSplitChance);
            newCubes.Add(newCube);
        }

        return newCubes;
    }
}