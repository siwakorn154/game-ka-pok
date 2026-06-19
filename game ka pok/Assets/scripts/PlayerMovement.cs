using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Camera cam;
    private Vector2 mousePos;

    [Header("ตั้งค่าการเคลื่อนที่")]
    public float moveSpeed = 5f; // ความเร็วของตัวละคร

    [Header("การอ้างอิง")]
    public Rigidbody2D rb; // ตัวอ้างอิงถึง Rigidbody 2D

    private Vector2 movement; // ตัวแปรเก็บทิศทางการเดินแกน X และ Y

    // Update ถูกเรียกใช้ทุกเฟรม: เหมาะกับการรับค่าปุ่มกด (Input) ที่สุด
    void Update()
    {
        // รับค่าปุ่มลูกศร หรือ W, A, S, D
        // จะได้ค่าระหว่าง -1 ถึง 1
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalize ทำให้การเดินเฉียงไม่เร็วกว่าปกติ (หากไม่ใส่ ตอนกด W+D พร้อมกันตัวละครจะวิ่งเร็วขึ้น)
        movement = movement.normalized;

        // แปลงตำแหน่งเมาส์บนหน้าจอให้เป็นตำแหน่งในโลกของเกม
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    // FixedUpdate ถูกเรียกใช้ตามรอบเวลาของฟิสิกส์: ต้องใช้เมื่อขยับ Rigidbody เสมอ
    void FixedUpdate()
    {
        // คำนวณตำแหน่งใหม่ = ตำแหน่งปัจจุบัน + (ทิศทาง * ความเร็ว * เวลาของฟิสิกส์)
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        // คำนวณเวกเตอร์ทิศทางจากตัวละครไปหาเมาส์
        Vector2 lookDir = mousePos - rb.position;
        // คำนวณองศาการหมุน (Atan2 คืนค่าเป็นเรเดียน ต้องคูณ Rad2Deg เพื่อแปลงเป็นองศา)
        // หมายเหตุ: -90f ด้านหลังขึ้นอยู่กับว่ารูป Sprite ต้นฉบับของคุณหันหน้าไปทางไหน (ปรับแก้ได้)
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f; 

        // สั่งให้ Rigidbody หมุนตามองศาที่คำนวณได้
        rb.rotation = angle;
    }
}