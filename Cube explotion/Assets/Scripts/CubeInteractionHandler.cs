using UnityEngine;

public class CubeInteractionHandler : MonoBehaviour
{
    public static CubeInteractionHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        InputHandler.Instance.OnCubeClicked += HandleCubeInteraction;
    }

    private void OnDestroy()
    {
        if (InputHandler.Instance != null)
            InputHandler.Instance.OnCubeClicked -= HandleCubeInteraction;
    }

    private void HandleCubeInteraction(Cube cube)
    {
        bool shouldSplit = Random.Range(0f, 1f) <= cube.SplitChance;
        Vector3 interactionPoint = cube.transform.position;

        if (shouldSplit && CanSplit(cube))
        {
            int newCubeCount = Random.Range(2, 7);
            CubeSpawner.Instance.SpawnMultipleCubes(cube, interactionPoint, newCubeCount);

            foreach (Cube newCube in FindObjectsOfType<Cube>())
            {
                if (newCube != cube)
                {
                    CubeExploder.Instance.ApplyExplosionToCube(newCube, interactionPoint);
                }
            }
        }
        else
        {

            CubeExploder.Instance.ApplyExplosionToCube(cube, interactionPoint);
        }
    }

    private bool CanSplit(Cube cube)
    {
        return cube.OriginalScale.x > 0.2f;
    }
}