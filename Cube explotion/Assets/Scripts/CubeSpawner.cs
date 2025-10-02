using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public static CubeSpawner Instance { get; private set; }

    [SerializeField] private GameObject _cubePrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public Cube SpawnCube(Vector3 position, Vector3 scale, float splitChance)
    {
        GameObject cubeObject = Instantiate(_cubePrefab, position, Quaternion.identity);
        cubeObject.name = "Cube";
        cubeObject.transform.localScale = scale;

        Cube cube = cubeObject.GetComponent<Cube>();
        cube.Initialize(splitChance, scale);

        cube.OnCubeClicked += HandleCubeClicked;

        return cube;
    }

    private void HandleCubeClicked(Cube cube)
    {
        cube.OnCubeClicked -= HandleCubeClicked;
        Destroy(cube.gameObject);
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