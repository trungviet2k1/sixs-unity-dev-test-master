using UnityEngine;

public class GameApplication : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
}