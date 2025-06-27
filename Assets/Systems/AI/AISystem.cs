using UnityEngine;

/// <summary>
/// AI system for opponent player
/// </summary>
[RequireComponent(typeof(PlayerMovement))]
public sealed class AISystem : MonoBehaviour
{
    [Header("AI Settings")]
    [SerializeField] private float reactionTime = 0.2f;
    [SerializeField] private float accuracy = 0.8f;
    [SerializeField] private float aggressiveness = 0.7f;

    private PlayerMovement movement;
    private Transform puck;
    private Vector2 targetPosition;
    private float lastDecisionTime;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        var puckController = FindFirstObjectByType<PuckController>();
        if (puckController != null)
        {
            puck = puckController.transform;
        }
        else
        {
            Debug.LogWarning("AISystem: No PuckController found in the scene.");
        }
        #pragma warning disable UNT0008 // Null propagation on Unity objects
        puck = FindFirstObjectByType<PuckController>(FindObjectsInactive.Include)?.transform;
    }

    private void Update()
    {
        if (puck == null) return;
        if (Time.time - lastDecisionTime > reactionTime)
        {
            MakeDecision();
            lastDecisionTime = Time.time;
        }
        MoveToTarget();
    }

    private void MakeDecision()
    {
        Vector2 puckPos = puck.position;
        Vector2 myPos = transform.position;

        // Predict puck movement
        Rigidbody2D puckRb = puck.GetComponent<Rigidbody2D>();
        Vector2 predictedPos = puckPos;
        if (puckRb != null)
        {
            predictedPos += puckRb.linearVelocity * reactionTime;
        }

        // Add inaccuracy
        Vector2 offset = Random.insideUnitCircle * (1f - accuracy);
        targetPosition = predictedPos + offset;
    }

    private void MoveToTarget()
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        // Direct velocity control for AI
        if (movement != null)
        {
            movement.SetVelocity(aggressiveness * movement.MoveSpeed * direction);
        }
    }
}