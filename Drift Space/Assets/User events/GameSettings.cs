using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;

    public float viewSlider = 1f;
    public float soundVolumeSlider = 1f;

    public float SafeSliderValue
    {
        get
        {
            if (viewSlider < 0f || viewSlider > 2f)
                return 1f;

            return viewSlider;
        }
    }

    public float SafeSoundVolume
    {
        get
        {
            if (soundVolumeSlider < 0f || soundVolumeSlider > 1f)
                return 1f;

            return soundVolumeSlider;
        }
    }

    public void ApplySoundVolume()
    {
        AudioListener.volume = SafeSoundVolume;
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}