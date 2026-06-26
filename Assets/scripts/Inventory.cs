using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public int inventorySize = 20;
    public List<InventorySlot> slots = new List<InventorySlot>();

    public UnityEvent OnInventoryChanged;

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < inventorySize; i++)
            slots.Add(new InventorySlot());
    }

    // หยิบของ → เข้า Hotbar ก่อน ถ้าเต็มค่อยเข้า Bag
    public bool AddItem(ItemData item, int amount = 1)
    {
        if (Hotbar.Instance != null && !Hotbar.Instance.IsFull())
        {
            bool addedToHotbar = Hotbar.Instance.AddItem(item, amount);
            if (addedToHotbar) return true;
        }

        return AddItemDirect(item, amount);
    }

    // ใส่ตรงเข้า Bag เลย ไม่ผ่าน Hotbar
    public bool AddItemDirect(ItemData item, int amount = 1)
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
                    if (amount <= 0) { OnInventoryChanged?.Invoke(); return true; }
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
                if (amount <= 0) { OnInventoryChanged?.Invoke(); return true; }
            }
        }

        OnInventoryChanged?.Invoke();
        return false;
    }

    public void RemoveItem(int slotIndex, int amount = 1)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count) return;
        InventorySlot slot = slots[slotIndex];
        if (slot.IsEmpty()) return;

        slot.quantity -= amount;
        if (slot.quantity <= 0) slot.Clear();

        OnInventoryChanged?.Invoke();
    }

    public void SwapSlots(int indexA, int indexB)
    {
        InventorySlot temp = slots[indexA];
        slots[indexA] = slots[indexB];
        slots[indexB] = temp;
        OnInventoryChanged?.Invoke();
    }
}