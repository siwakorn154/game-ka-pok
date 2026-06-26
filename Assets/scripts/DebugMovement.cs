using UnityEngine;

public class DebugMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude > 0.01f)
        {
            Debug.Log("Velocity: " + rb.linearVelocity);
        }
    }
}