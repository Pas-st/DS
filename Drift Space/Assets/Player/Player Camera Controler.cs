using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public enum CameraTurnMode
    {
        WorldFixed = 0,
        FollowTarget = 1,
        RotateToButton = 2
    }

    public CameraTurnMode cameraTurnMode = CameraTurnMode.WorldFixed;

    public Transform player;
    public RotateToNearestButton target;

    void Update()
    {
        // 👉 ONLY MODE FROM SLIDER
        if (GameSettings.Instance != null)
        {
            int mode = Mathf.RoundToInt(GameSettings.Instance.SafeSliderValue);
            mode = Mathf.Clamp(mode, 0, 2);

            cameraTurnMode = (CameraTurnMode)mode;
        }

        switch (cameraTurnMode)
        {
            case CameraTurnMode.WorldFixed:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;

            case CameraTurnMode.FollowTarget:
                if (player != null)
                    transform.rotation = player.rotation;
                break;

            case CameraTurnMode.RotateToButton:
                RotateToButton();
                break;
        }
    }

    void RotateToButton()
    {
        if (player == null || target == null)
            return;

        Vector2 dir = (target.transform.position - player.position).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion playerRotation = Quaternion.Euler(0, 0, angle - 90f);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            playerRotation,
            Time.deltaTime * 5f
        );
    }
}