using UnityEngine;
using UnityEngine.UI;

public class ViewSlider : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        if (GameSettings.Instance == null)
        {
            Debug.LogError("GameSettings fehlt in der Szene!");
            return;
        }

        if (slider == null)
        {
            Debug.LogError("Slider ist nicht zugewiesen!");
            return;
        }

        slider.value = GameSettings.Instance.viewSlider;
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnSliderChanged(float value)
    {
        GameSettings.Instance.viewSlider = value;
    }
}