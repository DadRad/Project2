using UnityEngine;

public class InputReader : MonoBehaviour
{
    public System.Action<Vector2> OnClickPerformed;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClickPerformed?.Invoke(Input.mousePosition);
        }
    }
}