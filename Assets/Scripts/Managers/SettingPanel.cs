using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    public static SettingPanel Instance { get; private set; }

    [Header("Settings Panel UI")]
    public GameObject settingsPanel;

    [Header("Audio Settings")]
    [SerializeField] private AudioMixer audioSettingMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    private const string MusicVolumeKey = "musicVolume";
    private const string SFXVolumeKey = "sfxVolume";

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

    void Start()
    {
        LoadDataSetting();
        musicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
        SFXSlider.onValueChanged.AddListener(delegate { SetSFXVolume(); });
    }

    public void OpenSettingPanel()
    {
        settingsPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseSettingPanel()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void SetMusicVolume()
    {
        SetVolume(musicSlider.value, MusicVolumeKey, "musicVolume");
    }

    public void SetSFXVolume()
    {
        SetVolume(SFXSlider.value, SFXVolumeKey, "sfxVolume");
    }

    private void SetVolume(float sliderValue, string playerPrefKey, string mixerParameter)
    {
        audioSettingMixer.SetFloat(mixerParameter, Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat(playerPrefKey, sliderValue);
    }

    public void LoadDataSetting()
    {
        LoadVolumeSetting(MusicVolumeKey, musicSlider, "musicVolume");
        LoadVolumeSetting(SFXVolumeKey, SFXSlider, "sfxVolume");
    }

    private void LoadVolumeSetting(string playerPrefKey, Slider slider, string mixerParameter)
    {
        if (PlayerPrefs.HasKey(playerPrefKey))
        {
            float volume = PlayerPrefs.GetFloat(playerPrefKey);
            slider.value = volume;
            audioSettingMixer.SetFloat(mixerParameter, Mathf.Log10(volume) * 20);
        }
    }
}