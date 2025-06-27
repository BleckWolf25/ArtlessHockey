// GameTimer.cs
using UnityEngine;
using System;

public sealed class GameTimer : MonoBehaviour
{
    public static event Action OnTimeUp;

    [SerializeField] private float gameDuration = 180f; // 3 minutes

    public float TimeRemaining { get; private set; }
    public bool IsRunning { get; private set; }

    private void Start()
    {
        TimeRemaining = gameDuration;
    }

    private void Update()
    {
        if (!IsRunning)
        {
            return;
        }

        TimeRemaining -= Time.deltaTime;

        if (TimeRemaining <= 0)
        {
            TimeRemaining = 0;
            IsRunning = false;
            OnTimeUp?.Invoke();
        }
    }

    public void StartTimer()
    {
        IsRunning = true;
    }

    public void PauseTimer()
    {
        IsRunning = false;
    }

    public void ResetTimer()
    {
        TimeRemaining = gameDuration;
    }
}
