using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private PlayerInputController input;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInputController>();
    }

    private void FixedUpdate()
    {
        Vector2 move = input.MoveInput.normalized;
        rb.linearVelocity = move * moveSpeed;
    }
}