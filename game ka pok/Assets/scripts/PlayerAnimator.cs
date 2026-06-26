using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 vel = rb.linearVelocity;
        bool isMoving = vel.magnitude > 0.1f;

        anim.SetBool("isMoving", isMoving);

        // อัพเดททิศทางเฉพาะตอนเดิน
        if (isMoving)
        {
            anim.SetFloat("moveX", vel.x);
            anim.SetFloat("moveY", vel.y);
        }
    }
}