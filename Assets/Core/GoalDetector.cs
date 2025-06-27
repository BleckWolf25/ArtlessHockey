// GoalDetector.cs
using UnityEngine;

public sealed class GoalDetector : MonoBehaviour
{
    [SerializeField] private bool isPlayerGoal;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Puck"))
        {
            if (isPlayerGoal)
            {
                ScoreManager.Instance.AddPlayerScore();
            }
            else
            {
                ScoreManager.Instance.AddAIScore();
            }

            ResetPuck();
        }
    }

    private void ResetPuck()
    {
        PuckController puck = FindFirstObjectByType<PuckController>();
        puck.transform.position = Vector3.zero;
        puck.Stop();
    }
}