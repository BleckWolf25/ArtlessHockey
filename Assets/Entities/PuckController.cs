using UnityEngine;
using System;

/// <summary>
/// Physics-driven puck controller with realistic hockey mechanics
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public sealed class PuckController : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private float maxVelocity = 15f;
    [SerializeField] private float friction = 0.98f;
    [SerializeField] private float bounceForce = 0.8f;

    // Events
    public static event Action<Collision2D> OnPuckCollision;
    public static event Action<float> OnVelocityChanged;

    private Rigidbody2D rb;

    public Vector2 Velocity => rb.linearVelocity;
    public float Speed => rb.linearVelocity.magnitude;
    public bool IsMoving => Speed > 0.1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ConfigurePhysics();
    }

    private void ConfigurePhysics()
    {
        rb.gravityScale = 0f;
        rb.linearDamping = 0f; // Manual friction control
        rb.angularDamping = 5f;
    }

    private void FixedUpdate()
    {
        ApplyFriction();
        ClampVelocity();
        NotifyVelocityChange();
    }

    private void ApplyFriction()
    {
        rb.linearVelocity *= friction;
    }

    private void ClampVelocity()
    {
        if (rb.linearVelocity.magnitude > maxVelocity)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxVelocity;
        }
    }

    private void NotifyVelocityChange()
    {
        OnVelocityChanged?.Invoke(Speed);
    }

    public void ApplyForce(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void SetVelocity(Vector2 velocity)
    {
        rb.linearVelocity = velocity;
    }

    public void Stop()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    [Header("Player Interaction")]
    [SerializeField] private float hitForceMultiplier = 5f;
    [SerializeField] private float maxHitForce = 20f;
    [SerializeField] private LayerMask playerLayer = 1 << 3; // Player layer

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnPuckCollision?.Invoke(collision);
        
        // Player hit handling (new code)
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            HandlePlayerHit(collision);
        }
        // Existing bounce code
        else
        {
            Vector2 reflection = Vector2.Reflect(rb.linearVelocity, collision.contacts[0].normal);
            rb.linearVelocity = reflection * bounceForce;
        }
    }

    private void HandlePlayerHit(Collision2D collision)
    {
        Rigidbody2D playerRb = collision.rigidbody;
        if (playerRb == null) return;

        Vector2 hitDirection = (transform.position - collision.transform.position).normalized;
        float playerSpeed = playerRb.linearVelocity.magnitude;
        float hitForce = Mathf.Min(playerSpeed * hitForceMultiplier, maxHitForce);
        
        ApplyForce(hitDirection * hitForce);
    }
}

// ...existing code...