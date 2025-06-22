using UnityEngine;

public class PuckRenderer : MonoBehaviour
{
    public float radius = 0.3f;
    public Color color = Color.black;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
    }
}