using UnityEngine;

// ส่งตัวนี้ให้ ItemAction ใช้แทนการอ้างอิง Player ตรงๆ
public class PlayerContext : MonoBehaviour
{
    public SpriteRenderer playerRenderer;
    public SpriteRenderer weaponRenderer;
    public Transform slashEffect;
    public SpriteRenderer slashRenderer;
    public Animator animator; // ถ้ามีในอนาคต

    public Sprite defaultSprite;  // รูปตอนมือเปล่า
}