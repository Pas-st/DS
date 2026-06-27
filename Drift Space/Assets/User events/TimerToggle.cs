using UnityEngine;
using UnityEngine.UI;

public class TimerToggle : MonoBehaviour
{
    public Toggle toggle;

    void Start()
    {
        if (GameSettings.Instance == null)
        {
            Debug.LogError("GameSettings fehlt in der Szene!");
            return;
        }

        toggle.isOn = GameSettings.Instance.showSpeedrunTimer;
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnToggleChanged(bool value)
    {
        GameSettings.Instance.showSpeedrunTimer = value;
    }
}