// AudioManager.cs
using UnityEngine;

public sealed class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip goalSound;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        PuckController.OnPuckCollision += _ => PlayHit();
        ScoreManager.OnGoalScored += _ => PlayGoal();
    }

    private void OnDisable()
    {
        PuckController.OnPuckCollision -= _ => PlayHit();
        ScoreManager.OnGoalScored -= _ => PlayGoal();
    }

    public void PlayHit()
    {
        sfxSource.PlayOneShot(hitSound);
    }

    public void PlayGoal()
    {
        sfxSource.PlayOneShot(goalSound);
    }
}
