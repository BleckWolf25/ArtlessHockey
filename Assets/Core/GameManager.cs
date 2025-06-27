using UnityEngine;
using System;

/// <summary>
/// Central game state manager implementing singleton pattern with lazy initialization
/// Handles game lifecycle, state transitions, and system coordination
/// </summary>
public sealed class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Events for state changes
    public static event Action<GameState> OnGameStateChanged;
    public static event Action OnGameStarted;
    public static event Action OnGamePaused;
    public static event Action OnGameEnded;

    [field: Header("Game State")]
    [field: SerializeField]
    public GameState CurrentState { get; private set; } = GameState.Menu;
    [field: Header("Game Configuration")]
    [field: SerializeField]
    public GameConfig Config { get; private set; }

    private void Awake()
    {
        // Singleton pattern with destroy duplicate
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeGame();
    }

    private void InitializeGame()
    {
        // Create default config if none assigned
        if (Config == null)
        {
            Config = ScriptableObject.CreateInstance<GameConfig>();
        }

        Debug.Log($"[GameManager] Initialized - Version: {Application.version}");

        // Initialize systems in dependency order
        InitializeSystems();
    }

    private void InitializeSystems()
    {
        // Systems initialization could be handled here
        // For now, just log the initialization
        Debug.Log("[GameManager] Core systems initialized");
    }

    /// <summary>
    /// Changes game state with event notification
    /// </summary>
    public void ChangeGameState(GameState newState)
    {
        if (CurrentState == newState)
        {
            return;
        }

        GameState previousState = CurrentState;
        CurrentState = newState;

        Debug.Log($"[GameManager] State changed: {previousState} -> {newState}");

        OnGameStateChanged?.Invoke(newState);

        // Trigger specific state events
        switch (newState)
        {
            case GameState.Playing:
                OnGameStarted?.Invoke();
                break;
            case GameState.Paused:
                OnGamePaused?.Invoke();
                break;
            case GameState.GameOver:
                OnGameEnded?.Invoke();
                break;
            case GameState.Menu:
                break;
            default:
                break;
        }
    }

    public void StartGame()
    {
        ChangeGameState(GameState.Playing);
    }

    public void PauseGame()
    {
        ChangeGameState(GameState.Paused);
    }

    public void EndGame()
    {
        ChangeGameState(GameState.GameOver);
    }

    public void ReturnToMenu()
    {
        ChangeGameState(GameState.Menu);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}

/// <summary>
/// Game state enumeration
/// </summary>
public enum GameState
{
    Menu,
    Playing,
    Paused,
    GameOver
}

/// <summary>
/// Scriptable object for game configuration
/// </summary>
[CreateAssetMenu(fileName = "GameConfig", menuName = "Artless Hockey/Game Config")]
public class GameConfig : ScriptableObject
{
    [Header("Gameplay")]
    public float gameTimeMinutes = 3f;
    public int maxScore = 5;

    [Header("Physics")]
    public float puckFriction = 0.98f;
    public float puckBounce = 0.8f;

    [Header("Performance")]
    public int targetFrameRate = 60;
    public bool enableVSync = true;
}