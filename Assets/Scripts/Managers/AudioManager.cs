using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("----- Audio Source -----")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("----- Audio Clip -----")]
    [SerializeField] AudioClip background;
    [SerializeField] AudioClip kickBall;
    [SerializeField] AudioClip goals;

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

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayKickBallSound()
    {
        PlaySFX(kickBall);
    }

    public void PlayScoringGoalSound()
    {
        PlaySFX(goals);
    }
}