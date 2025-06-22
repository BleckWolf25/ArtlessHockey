using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PuckController : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyForce(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }
}