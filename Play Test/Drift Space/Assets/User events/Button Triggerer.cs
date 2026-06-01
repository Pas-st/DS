using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(LineRenderer))]
public class OpenLock : MonoBehaviour
{
    public Transform player;
    public Vector2 DoorPosition;
    public Vector2 buttonPosition;
    public Vector2 buttonSize;

    public Tilemap doorTilemap;

    private RotateToNearestButton rotator;
    private bool activated = false;

    private LineRenderer line;

    void Start()
    {
        rotator = FindFirstObjectByType<RotateToNearestButton>();

        line = GetComponent<LineRenderer>();

        // LineRenderer basic setup
        line.positionCount = 2;
        line.startWidth = 0.05f;
        line.endWidth = 0.05f;
        line.useWorldSpace = true;
    }

    void Update()
    {
        if (!activated)
        {
            DrawLineToDoor();
        }

        if (activated) return;

        if (IsPlayerInside())
        {
            Activate();
        }
    }

    bool IsPlayerInside()
    {
        return player.position.x > buttonPosition.x - buttonSize.x / 2 &&
               player.position.x < buttonPosition.x + buttonSize.x / 2 &&
               player.position.y > buttonPosition.y - buttonSize.y / 2 &&
               player.position.y < buttonPosition.y + buttonSize.y / 2;
    }

    void Activate()
    {
        activated = true;

        // Linie ausblenden
        line.enabled = false;

        // Tür öffnen
        if (doorTilemap != null)
            doorTilemap.gameObject.SetActive(false);

        // Rotator weiter schalten
        if (rotator != null)
            rotator.NextButton();
    }

    void DrawLineToDoor()
    {
        if (line == null) return;

        line.enabled = true;

        line.SetPosition(0, new Vector3(buttonPosition.x, buttonPosition.y, 0));
        line.SetPosition(1, new Vector3(DoorPosition.x, DoorPosition.y, 0));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(
            new Vector3(buttonPosition.x, buttonPosition.y, 0),
            new Vector3(buttonSize.x, buttonSize.y, 0)
        );
    }
}