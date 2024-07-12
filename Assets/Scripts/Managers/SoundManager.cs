using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Background Music")]
    public AudioSource backgroundMusic;

    [Header("Sound Effects")]
    public AudioClip kickBallSoundEffect;
    public AudioClip goalsSoundEffect;

    [Header("Audio Sources")]
    public AudioSource kickBallSound;
    public AudioSource goalsSound;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayKickBallSound()
    {
        kickBallSound.PlayOneShot(kickBallSoundEffect);
    }

    public void PlayScoringGoalSound()
    {
        goalsSound.PlayOneShot(goalsSoundEffect);
    }
}