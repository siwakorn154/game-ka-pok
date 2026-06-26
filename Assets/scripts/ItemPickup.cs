using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData item;
    public int quantity = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // เช็คก่อนว่า item ถูก assign หรือยัง
        if (item == null)
        {
            Debug.LogError("ItemPickup: ยังไม่ได้ลาก Item มาใส่ใน Inspector! (" + gameObject.name + ")");
            return;
        }

        if (Inventory.Instance == null)
        {
            Debug.LogError("ไม่พบ Inventory Instance! ตรวจสอบว่า InventoryManager อยู่ในฉากและมีสคริปต์ Inventory");
            return;
        }

        bool added = Inventory.Instance.AddItem(item, quantity);
        Debug.Log(added ? $"หยิบ {item.itemName} สำเร็จ!" : "กระเป๋าเต็ม!");

        if (added) Destroy(gameObject);
    }
}