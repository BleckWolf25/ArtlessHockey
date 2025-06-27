
// UIManager.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public sealed class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;

    private void OnEnable()
    {
        ScoreManager.OnScoreChanged += UpdateScore;
        GameTimer.OnTimeUp += ShowGameOver;
        restartButton.onClick.AddListener(RestartGame);
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreChanged -= UpdateScore;
        GameTimer.OnTimeUp -= ShowGameOver;
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateScore(int playerScore, int aiScore)
    {
        scoreText.text = $"{playerScore} : {aiScore}";
    }

    private void UpdateTimer()
    {
        GameTimer timer = FindFirstObjectByType<GameTimer>();
        if (timer != null)
        {
            int minutes = Mathf.FloorToInt(timer.TimeRemaining / 60);
            int seconds = Mathf.FloorToInt(timer.TimeRemaining % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    private void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        GameManager.Instance.EndGame();
    }

    private void RestartGame()
    {
        gameOverPanel.SetActive(false);
        ScoreManager.Instance.ResetScore();
        FindFirstObjectByType<GameTimer>().ResetTimer();
        GameManager.Instance.StartGame();
    }

    private GameTimer timer;
    void Start() => timer = FindFirstObjectByType<GameTimer>();
}