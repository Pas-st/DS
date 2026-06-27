using UnityEngine;
using UnityEngine.UI;

public class SpeedToggle : MonoBehaviour
{
    public Toggle toggle;

    void Start()
    {
        if (GameSettings.Instance == null)
        {
            Debug.LogError("GameSettings fehlt in der Szene!");
            return;
        }

        toggle.isOn = GameSettings.Instance.showPlayerSpeed;
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnToggleChanged(bool value)
    {
        GameSettings.Instance.showPlayerSpeed = value;
    }
}