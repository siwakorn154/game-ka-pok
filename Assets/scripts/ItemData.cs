using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    [TextArea] public string description;
    public bool isStackable = true;
    public int maxStackAmount = 99;

    [Header("Weapon")]
    public bool isWeapon = false;
    public Sprite playerHoldSprite;  // รูปตัวละครตอนถือ
    public float attackDamage = 10f;
    public float attackCooldown = 0.4f;
}