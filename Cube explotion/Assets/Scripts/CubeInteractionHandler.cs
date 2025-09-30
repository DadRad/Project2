using UnityEngine;

public class CubeInteractionHandler : MonoBehaviour
{
    public static CubeInteractionHandler Instance { get; private set; }

    [SerializeField] private float _explosionForce = 10f;
    [SerializeField] private LayerMask _cubeLayerMask;
    [SerializeField] private Camera _mainCamera;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (_mainCamera == null)
            _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }
    }

    private void HandleMouseClick()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _cubeLayerMask))
        {
            Cube cube = hit.collider.GetComponent<Cube>();

            if (cube != null)
            {
                HandleCubeInteraction(cube, hit.point);
            }
        }
    }

    public void HandleCubeInteraction(Cube cube, Vector3 hitPoint)
    {
        bool shouldSplit = Random.Range(0f, 1f) <= cube.SplitChance;

        if (shouldSplit && CanSplit(cube))
        {
            SplitCube(cube, hitPoint);
        }
        else
        {
            Rigidbody rb = cube.GetComponent<Rigidbody>();

            if (rb != null)
            {
                ApplyExplosionForce(rb, hitPoint);
            }
        }

        Destroy(cube.gameObject);
    }

    private bool CanSplit(Cube cube)
    {
        return cube.OriginalScale.x > 0.2f;
    }

    private void SplitCube(Cube originalCube, Vector3 explosionOrigin)
    {
        int newCubeCount = Random.Range(2, 7);

        for (int i = 0; i < newCubeCount; i++)
        {
            CreateNewCube(originalCube, explosionOrigin);
        }
    }

    private void CreateNewCube(Cube originalCube, Vector3 explosionOrigin)
    {
        GameObject newCube = Instantiate(originalCube.gameObject);
        newCube.name = "Cube";

        Vector3 randomOffset = Random.insideUnitSphere * 0.5f;
        newCube.transform.position = explosionOrigin + randomOffset;

        Vector3 newScale = originalCube.OriginalScale * 0.5f;
        newCube.transform.localScale = newScale;

        Cube newCubeScript = newCube.GetComponent<Cube>();
        newCubeScript.Initialize(originalCube.SplitChance * 0.5f, newScale);

        Rigidbody newRb = newCube.GetComponent<Rigidbody>();

        if (newRb == null)
        {
            newRb = newCube.AddComponent<Rigidbody>();
        }

        newRb.useGravity = true;
        newRb.mass = newScale.x;

        ApplyExplosionForce(newRb, explosionOrigin);
    }

    private void ApplyExplosionForce(Rigidbody targetRb, Vector3 explosionOrigin)
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
}