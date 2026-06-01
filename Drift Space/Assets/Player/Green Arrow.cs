using System.Collections.Generic;
using UnityEngine;

public class RotateToNearestButton : MonoBehaviour
{
    public Transform player;
    public float radius = 2f;
    public float moveSpeed = 8f;

    public List<Vector2> buttonPositions = new List<Vector2>();

    private int currentIndex = 0;

    void Update()
    {
        if (player == null || buttonPositions.Count == 0)
            return;

        currentIndex = Mathf.Clamp(currentIndex, 0, buttonPositions.Count - 1);

        Vector2 target = buttonPositions[currentIndex];
        Vector2 playerPos = player.position;

        // Richtung Spieler → Button
        Vector2 dir = (target - playerPos).normalized;

        // Winkel bestimmen
        float angle = Mathf.Atan2(dir.y, dir.x);

        // Kreisposition um Spieler
        Vector2 desiredPos = playerPos + new Vector2(
            Mathf.Cos(angle),
            Mathf.Sin(angle)
        ) * radius;

        transform.position = Vector2.Lerp(
            transform.position,
            desiredPos,
            Time.deltaTime * moveSpeed
        );

        // immer auf Button schauen
        Vector2 lookDir = (target - (Vector2)transform.position).normalized;
        float rot = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rot-90);
    }

    // 👇 sauberer Übergang zum nächsten Button
    public void NextButton()
    {
        currentIndex++;

        if (currentIndex >= buttonPositions.Count)
        {
            currentIndex = buttonPositions.Count - 1;
            Debug.Log("Alle Buttons aktiviert!");
        }
    }
}