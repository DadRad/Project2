using UnityEngine;

public class Raycaster : MonoBehaviour
{
    [SerializeField] private LayerMask _cubeLayerMask;
    [SerializeField] private Camera _mainCamera;

    public System.Action<Cube> OnCubeHit;

    private void Awake()
    {
        if (_mainCamera == null)
            _mainCamera = Camera.main;
    }

    public void PerformRaycast(Vector2 screenPosition)
    {
        Ray ray = _mainCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _cubeLayerMask))
        {
            if (hit.collider.TryGetComponent(out Cube cube))
            {
                OnCubeHit?.Invoke(cube);
            }
        }
    }
}