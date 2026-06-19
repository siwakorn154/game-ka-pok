using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // ลาก Player มาใส่
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0, 0, -10); // Z = -10 สำหรับ 2D

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
    }
}