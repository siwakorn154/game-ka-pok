using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hotbar : MonoBehaviour
{
    public static Hotbar Instance;

    public int hotbarSize = 10;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public int selectedIndex = 0;

    public UnityEvent OnHotbarChanged;
    public UnityEvent OnSelectionChanged;

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < hotbarSize; i++)
            slots.Add(new InventorySlot());
    }

    public bool AddItem(ItemData item, int amount = 1)
    {
        if (item.isStackable)
        {
            foreach (var slot in slots)
            {
                if (!slot.IsEmpty() && slot.item == item && slot.quantity < item.maxStackAmount)
                {
                    int space = item.maxStackAmount - slot.quantity;
                    int add = Mathf.Min(space, amount);
                    slot.quantity += add;
                    amount -= add;
                    if (amount <= 0) { OnHotbarChanged?.Invoke(); return true; }
                }
            }
        }

        foreach (var slot in slots)
        {
            if (slot.IsEmpty())
            {
                slot.item = item;
                slot.quantity = Mathf.Min(amount, item.isStackable ? item.maxStackAmount : 1);
                amount -= slot.quantity;
                if (amount <= 0) { OnHotbarChanged?.Invoke(); return true; }
            }
        }

        OnHotbarChanged?.Invoke();
        return false;
    }

    public bool IsFull()
    {
        foreach (var slot in slots)
            if (slot.IsEmpty()) return false;
        return true;
    }

    public void SetSelected(int index)
    {
        selectedIndex = Mathf.Clamp(index, 0, hotbarSize - 1);
        OnSelectionChanged?.Invoke();
    }

    // ย้ายจาก Hotbar → Bag
    public void MoveToInventory(int hotbarIndex)
    {
        var slot = slots[hotbarIndex];
        if (slot.IsEmpty()) return;

        bool added = Inventory.Instance.AddItemDirect(slot.item, slot.quantity);
        if (added)
        {
            slot.Clear();
            Debug.Log("ย้ายไป Bag สำเร็จ!");
        }
        else
        {
            Debug.Log("Bag เต็ม!");
        }

        OnHotbarChanged?.Invoke();
    }

    // ย้ายจาก Bag → Hotbar (สลับกับ selected slot)
    public void MoveFromInventory(int inventoryIndex)
    {
        var invSlot = Inventory.Instance.slots[inventoryIndex];
        if (invSlot.IsEmpty()) return;

        var hotSlot = slots[selectedIndex];

        if (hotSlot.IsEmpty())
        {
            hotSlot.item = invSlot.item;
            hotSlot.quantity = invSlot.quantity;
            invSlot.Clear();
        }
        else
        {
            // Swap
            ItemData tempItem = hotSlot.item;
            int tempQty = hotSlot.quantity;

            hotSlot.item = invSlot.item;
            hotSlot.quantity = invSlot.quantity;

            invSlot.item = tempItem;
            invSlot.quantity = tempQty;
        }

        OnHotbarChanged?.Invoke();
        Inventory.Instance.OnInventoryChanged?.Invoke();
    }
}