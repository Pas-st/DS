using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxFactor = 0.5f;

    private Vector3 lastCamPos;

    void Start()
    {
        lastCamPos = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 delta = cameraTransform.position - lastCamPos;
        transform.position += delta * parallaxFactor;
        lastCamPos = cameraTransform.position;
    }
}