using UnityEngine;
using TMPro;
using System.Diagnostics;

public class Timer : MonoBehaviour
{
    private Stopwatch stopwatch;
    private Move playerMove;
    private bool stopped = false;

    public TMP_Text timerText;

    void Start()
    {
        playerMove = GetComponent<Move>();

        stopwatch = new Stopwatch();
        stopwatch.Start();
    }

    void Update()
    {
        // Timer anzeigen
        timerText.text =
            $"{stopwatch.Elapsed.Minutes:D2}:{stopwatch.Elapsed.Seconds:D2}.{stopwatch.Elapsed.Milliseconds:D3}";

        // Timer stoppen
        if (!stopped && playerMove.levelComplete)
        {
            stopped = true;
            stopwatch.Stop();
        }
    }
}