using System;
using UnityEngine;

/// <summary>
/// Physics-based player movement with configurable parameters and smooth controls
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInputController))]
public sealed class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 15f;
    [SerializeField] private float maxSpeed = 8f;

    [Header("Physics")]
    [SerializeField] private float dragCoefficient = 2f;
    [SerializeField] private bool useSmoothing = true;

    // Components
    private Rigidbody2D rb;
    private PlayerInputController inputController;

    // State
    private Vector2 targetVelocity;
    private Vector2 currentInput;

    // Properties
    public Vector2 CurrentVelocity => rb.linearVelocity;
    public float CurrentSpeed => rb.linearVelocity.magnitude;
    public bool IsMoving => CurrentSpeed > 0.1f;
    public float MoveSpeed => moveSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputController = GetComponent<PlayerInputController>();

        // Configure Rigidbody2D
        rb.linearDamping = dragCoefficient;
        rb.angularDamping = 5f;
        rb.gravityScale = 0f;
    }

    private void OnEnable()
    {
        PlayerInputController.OnMoveInputChanged += HandleInputChanged;
    }

    private void OnDisable()
    {
        PlayerInputController.OnMoveInputChanged -= HandleInputChanged;
    }

    private void HandleInputChanged(Vector2 input)
    {
        currentInput = input;
        targetVelocity = input.normalized * moveSpeed;
    }

    private void FixedUpdate()
    {
        UpdateMovement();
        ClampVelocity();
    }

    private void UpdateMovement()
    {
        if (useSmoothing)
        {
            ApplySmoothMovement();
        }
        else
        {
            ApplyDirectMovement();
        }
    }

    private void ApplySmoothMovement()
    {
        Vector2 currentVelocity = rb.linearVelocity;
        float deltaTime = Time.fixedDeltaTime;

        if (currentInput.magnitude > 0)
        {
            // Accelerate towards target velocity
            Vector2 velocityDifference = targetVelocity - currentVelocity;
            Vector2 accelerationForce = acceleration * deltaTime * velocityDifference;
            rb.linearVelocity = currentVelocity + accelerationForce;
        }
        else
        {
            // Decelerate when no input
            Vector2 decelerationForce = deceleration * deltaTime * -currentVelocity;
            rb.linearVelocity = currentVelocity + decelerationForce;
        }
    }

    private void ApplyDirectMovement()
    {
        rb.linearVelocity = targetVelocity;
    }

    private void ClampVelocity()
    {
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    /// <summary>
    /// Apply external force to the player (e.g., collisions, power-ups)
    /// </summary>
    public void ApplyForce(Vector2 force, ForceMode2D mode = ForceMode2D.Impulse)
    {
        rb.AddForce(force, mode);
    }

    /// <summary>
    /// Instantly set player velocity
    /// </summary>
    public void SetVelocity(Vector2 velocity)
    {
        rb.linearVelocity = velocity;
    }

    /// <summary>
    /// Stop player movement immediately
    /// </summary>
    public void Stop()
    {
        rb.linearVelocity = Vector2.zero;
        targetVelocity = Vector2.zero;
        currentInput = Vector2.zero;
    }
}