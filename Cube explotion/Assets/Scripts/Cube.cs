using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour
{
    [SerializeField] private Vector3 _originalScale;
    [SerializeField] private float _splitChance = 1f;
    private Renderer _cubeRenderer;

    public Vector3 OriginalScale => _originalScale;
    public float SplitChance => _splitChance;

    private void Awake()
    {
        _cubeRenderer = GetComponent<Renderer>();
        _originalScale = transform.localScale;
    }

    private void Start()
    {
        SetRandomColor();
    }

    private void SetRandomColor()
    {
        if (_cubeRenderer != null)
        {
            _cubeRenderer.material.color = Random.ColorHSV();
        }
    }

    public void Initialize(float splitChance, Vector3 scale)
    {
        _splitChance = splitChance;
        _originalScale = scale;
        transform.localScale = scale;
    }
}