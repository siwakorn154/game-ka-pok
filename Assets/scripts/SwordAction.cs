using UnityEngine;

[CreateAssetMenu(fileName = "SwordAction", menuName = "Items/Actions/Sword")]
public class SwordAction : ItemAction
{
    public Sprite playerWithSwordSprite;  // รูปตัวละครตอนถือดาบ
    public float damage = 10f;
    public float cooldown = 0.4f;
    public float slashDistance = 0.6f;
    public float slashDisplayTime = 0.15f;

    private float lastAttackTime;

    public override void OnEquip(PlayerContext player)
    {
        // เปลี่ยนรูปตัวละครเป็นท่าถือดาบ
        if (playerWithSwordSprite != null)
            player.playerRenderer.sprite = playerWithSwordSprite;

        if (player.weaponRenderer != null)
            player.weaponRenderer.enabled = true;
    }

    public override void OnUse(PlayerContext player)
    {
        if (Time.time - lastAttackTime < cooldown) return;
        lastAttackTime = Time.time;

        // หาทิศจากเม้า
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        Vector2 dir = SnapToFour((mouseWorld - player.transform.position).normalized);

        // แสดง Slash
        ShowSlash(player, dir);

        // ตี Enemy
        Vector2 hitPos = (Vector2)player.transform.position + dir * slashDistance;
        var hits = Physics2D.OverlapBoxAll(hitPos, new Vector2(0.8f, 0.8f), 0);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
                Debug.Log($"ตี {hit.name} {damage} dmg");
                // hit.GetComponent<EnemyHealth>()?.TakeDamage(damage);
        }
    }

    public override void OnUnequip(PlayerContext player)
    {
        // คืนรูปตัวละครเป็นปกติ
        if (player.defaultSprite != null)
            player.playerRenderer.sprite = player.defaultSprite;

        if (player.weaponRenderer != null)
            player.weaponRenderer.enabled = false;

        if (player.slashRenderer != null)
            player.slashRenderer.enabled = false;
    }

    private Vector2 SnapToFour(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle >= -45f && angle < 45f)    return Vector2.right;
        if (angle >= 45f && angle < 135f)    return Vector2.up;
        if (angle >= 135f || angle < -135f)  return Vector2.left;
        return Vector2.down;
    }

    private void ShowSlash(PlayerContext player, Vector2 dir)
    {
        if (player.slashEffect == null) return;
        player.slashEffect.position = (Vector2)player.transform.position + dir * slashDistance;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        player.slashEffect.rotation = Quaternion.Euler(0, 0, angle);

        if (player.slashRenderer != null)
        {
            player.slashRenderer.enabled = true;
            // ซ่อนหลัง slashDisplayTime วิ
            player.StartCoroutine(HideSlashAfter(player, slashDisplayTime));
        }
    }

    private System.Collections.IEnumerator HideSlashAfter(PlayerContext player, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (player.slashRenderer != null)
            player.slashRenderer.enabled = false;
    }
}