using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;

    public float viewSlider = 1f;
    public float soundVolumeSlider = 1f;
    public bool showSpeedrunTimer = true;
    public bool showPlayerHealth = true;
    public bool showBlockCounter = true;
    public bool showPlayerSpeed = true;

    public int targetDisplay = 0;

    public bool SafeShowSpeedrunTimer
    {
        get
        {
            return showSpeedrunTimer;
        }
    }

    public bool SafeShowPlayerHealth
    {
        get
        {
            return showPlayerHealth;
        }
    }

    public bool SafeShowBlockCounter
    {
        get
        {
            return showBlockCounter;
        }
    }

    public bool SafeShowPlayerSpeed
    {
        get
        {
            return showPlayerSpeed;
        }
    }

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