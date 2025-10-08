using UnityEngine;

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

    public void SpawnMultipleCubes(Cube originalCube, Vector3 explosionOrigin, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randomOffset = Random.insideUnitSphere * 0.5f;
            Vector3 newScale = originalCube.OriginalScale * 0.5f;
            float newSplitChance = originalCube.SplitChance * 0.5f;

            SpawnCube(explosionOrigin + randomOffset, newScale, newSplitChance);
        }
    }
}