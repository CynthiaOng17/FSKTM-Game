using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 2, -10);

    [Header("Camera Bounds")]
    public float minX = -5f;
    public float maxX = 100f;
    public float minY = -5f;
    public float maxY = 10f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = target.position + offset;

        // Clamp camera position
        desired.x = Mathf.Clamp(desired.x, minX, maxX);
        desired.y = Mathf.Clamp(desired.y, minY, maxY);

        transform.position = Vector3.Lerp(
            transform.position, desired, smoothSpeed);
    }
}