using UnityEngine;

public class PlayerRenderer : MonoBehaviour
{
    public float radius = 0.5f;
    public Color color = Color.white;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
    }
}