using UnityEngine;
using TMPro;
using System.Diagnostics;

public class Speedrun : MonoBehaviour
{
    [Header("Timer")]
    private Stopwatch stopwatch;
    private Move playerMove;
    private bool stopped = false;

    public TMP_Text timerText;

    [Header("Speed")]
    public TMP_Text speedText;

    [Header("Life")]
    public TMP_Text healthText;

    void Start()
    {
        // Timer
        playerMove = GetComponent<Move>();
        stopwatch = new Stopwatch();
        stopwatch.Start();
        timerText.gameObject.SetActive(GameSettings.Instance.showSpeedrunTimer);

        // Speed
        speedText.gameObject.SetActive(GameSettings.Instance.showPlayerSpeed);

        // health
        healthText.gameObject.SetActive(GameSettings.Instance.showPlayerHealth);

    }

    void Update()
    {
        // Timer anzeigen
        timerText.text = $"{stopwatch.Elapsed.Minutes:D2}:{stopwatch.Elapsed.Seconds:D2}.{stopwatch.Elapsed.Milliseconds:D3}";
        // Timer stoppen
        if (!stopped && playerMove.levelComplete)
        {
            stopped = true;
            stopwatch.Stop();
        }

        // Speed anzeigen
        speedText.text = $"{playerMove.currentSpeed:F1} f";

        // Health anzeigen
        healthText.text = $"{playerMove.currentHealth} HP";
    }
}