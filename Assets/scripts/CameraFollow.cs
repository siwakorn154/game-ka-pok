using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public float smoothSpeed = 8f;

    [Header("Bounds — ลาก CameraBounds มาใส่")]
    public BoxCollider2D boundsCollider;

    private Camera cam;
    private float camHalfH;
    private float camHalfW;

    // ขอบเขตของกล้อง
    private float minX, maxX, minY, maxY;
    private bool hasBounds = false;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        if (boundsCollider != null)
        {
            hasBounds = true;
            CalculateBounds();
        }
    }

    private void CalculateBounds()
    {
        camHalfH = cam.orthographicSize;
        camHalfW = camHalfH * cam.aspect;

        Bounds b = boundsCollider.bounds;
        minX = b.min.x + camHalfW;
        maxX = b.max.x - camHalfW;
        minY = b.min.y + camHalfH;
        maxY = b.max.y - camHalfH;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // ตำแหน่งที่ต้องการ (ตามตัวละคร)
        Vector3 desired = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
        );

        // ถ้ามี bounds ให้ clamp ตำแหน่งกล้องไม่ให้เกินขอบ
        if (hasBounds)
        {
            desired.x = Mathf.Clamp(desired.x, minX, maxX);
            desired.y = Mathf.Clamp(desired.y, minY, maxY);
        }

        // เลื่อนกล้องอย่าง smooth
        transform.position = Vector3.Lerp(
            transform.position,
            desired,
            smoothSpeed * Time.deltaTime
        );
    }
}