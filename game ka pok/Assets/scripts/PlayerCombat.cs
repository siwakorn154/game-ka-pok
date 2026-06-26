using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer playerRenderer;
    public GameObject weaponPivot;
    public SpriteRenderer weaponRenderer;

    [Header("Sprites")]
    public Sprite defaultPlayerSprite;

    private float lastAttackTime;
    private Camera cam;
    private bool hasWeapon = false;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        Hotbar.Instance.OnSelectionChanged.AddListener(UpdateEquip);
        Hotbar.Instance.OnHotbarChanged.AddListener(UpdateEquip);
        UpdateEquip();
    }

    private void Update()
    {
        if (!hasWeapon) return;

        // หมุน WeaponPivot ตามเม้าตลอดเวลา
        RotateWeaponToMouse();

        // คลิกซ้ายเพื่อตี
        if (Input.GetMouseButtonDown(0))
            TryAttack();
    }

    // Debug แสดง hitbox ตลอดเวลา (ลบทิ้งตอน Build จริง)
private void OnDrawGizmos()
{
    if (!Application.isPlaying) return;
    var slot = Hotbar.Instance?.slots[Hotbar.Instance.selectedIndex];
    if (slot == null || slot.IsEmpty() || !slot.item.isWeapon) return;

    Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
    mouseWorld.z = 0;
    Vector2 snapped = SnapToFour((mouseWorld - transform.position).normalized);

    float blockSize = 1f;
    Vector2 center = (Vector2)transform.position + snapped * (blockSize * 2f);
    Vector2 size = (snapped == Vector2.left || snapped == Vector2.right)
        ? new Vector2(blockSize * 3f, blockSize)
        : new Vector2(blockSize, blockSize * 3f);

    Gizmos.color = new Color(1, 0, 0, 0.4f);
    Gizmos.DrawCube(center, size);
}

    // อัพเดทเมื่อเปลี่ยนช่อง Hotbar
    private void UpdateEquip()
    {
        var slot = Hotbar.Instance.slots[Hotbar.Instance.selectedIndex];

        if (!slot.IsEmpty() && slot.item.isWeapon)
        {
            // ถือดาบอยู่
            hasWeapon = true;
            weaponPivot.SetActive(true);

            // เปลี่ยนรูปตัวละคร
            if (slot.item.playerHoldSprite != null)
                playerRenderer.sprite = slot.item.playerHoldSprite;

            // ใส่รูปดาบ
            if (slot.item.icon != null)
                weaponRenderer.sprite = slot.item.icon;
        }
        else
        {
            // มือเปล่า
            hasWeapon = false;
            weaponPivot.SetActive(false);

            // คืนรูปตัวละครปกติ
            if (defaultPlayerSprite != null)
                playerRenderer.sprite = defaultPlayerSprite;
        }
    }

    private void RotateWeaponToMouse()
    {
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        Vector2 dir = (mouseWorld - transform.position).normalized;

        // Snap 4 ทิศ
        Vector2 snapped = SnapToFour(dir);
        float angle = Mathf.Atan2(snapped.y, snapped.x) * Mathf.Rad2Deg;
        weaponPivot.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void TryAttack()
{
    var slot = Hotbar.Instance.slots[Hotbar.Instance.selectedIndex];
    if (Time.time - lastAttackTime < slot.item.attackCooldown) return;
    lastAttackTime = Time.time;

    Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
    mouseWorld.z = 0;
    Vector2 snapped = SnapToFour((mouseWorld - transform.position).normalized);

    // Hitbox 3 block ในทิศนั้น
    // จุดกึ่งกลางของ 3 block = ห่างจาก player 2 block (เพราะ block แรกติดตัว)
    float blockSize = 1f; // 1 unit = 1 block
    Vector2 hitCenter = (Vector2)transform.position + snapped * (blockSize * 2f);
    Vector2 hitSize = new Vector2(blockSize, blockSize * 3f); // กว้าง 1 สูง 3

    // ถ้าตีแนวนอน ให้หมุน hitSize
    if (snapped == Vector2.left || snapped == Vector2.right)
        hitSize = new Vector2(blockSize * 3f, blockSize);

    // แสดง Debug (เห็นตอน Play)
    Debug.DrawLine(
        (Vector3)hitCenter - (Vector3)(hitSize * 0.5f),
        (Vector3)hitCenter + (Vector3)(hitSize * 0.5f),
        Color.red, 0.5f
    );

    // ชน Enemy
    Collider2D[] hits = Physics2D.OverlapBoxAll(hitCenter, hitSize, 0);
    foreach (var hit in hits)
    {
        if (hit.CompareTag("Enemy"))
            Debug.Log($"โดน {hit.name} {slot.item.attackDamage} dmg");
    }

    // เรียก Effect (เผื่อไว้ใส่ทีหลัง)
    OnAttackEffect(snapped, hitCenter, hitSize);
}

// ฟังก์ชันนี้เผื่อไว้ใส่ Effect ทีหลัง
private void OnAttackEffect(Vector2 dir, Vector2 center, Vector2 size)
{
    // TODO: เพิ่ม Slash Effect ตรงนี้ทีหลัง
}

    private Vector2 SnapToFour(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle >= -45f && angle < 45f)   return Vector2.right;
        if (angle >= 45f && angle < 135f)   return Vector2.up;
        if (angle >= 135f || angle < -135f) return Vector2.left;
        return Vector2.down;
    }
}