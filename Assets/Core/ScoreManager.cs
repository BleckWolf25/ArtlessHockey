// ScoreManager.cs
using UnityEngine;
using System;

public sealed class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public static event Action<int, int> OnScoreChanged;
    public static event Action<int> OnGoalScored;

    public int PlayerScore { get; private set; }
    public int AIScore { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void AddPlayerScore()
    {
        PlayerScore++;
        OnScoreChanged?.Invoke(PlayerScore, AIScore);
        OnGoalScored?.Invoke(0);
    }

    public void AddAIScore()
    {
        AIScore++;
        OnScoreChanged?.Invoke(PlayerScore, AIScore);
        OnGoalScored?.Invoke(1);
    }

    public void ResetScore()
    {
        PlayerScore = AIScore = 0;
        OnScoreChanged?.Invoke(0, 0);
    }
}
