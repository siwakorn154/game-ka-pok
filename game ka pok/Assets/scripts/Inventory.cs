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
        {
            slots.Add(new InventorySlot());
        }
    }

    public bool AddItem(ItemData item, int amount = 1)
    {
        // ลองรวมกับช่องที่มีของชนิดเดียวกันก่อน (ถ้า stack ได้)
        if (item.isStackable)
        {
            foreach (var slot in slots)
            {
                if (!slot.IsEmpty() && slot.item == item && slot.quantity < item.maxStackAmount)
                {
                    int spaceLeft = item.maxStackAmount - slot.quantity;
                    int amountToAdd = Mathf.Min(spaceLeft, amount);
                    slot.quantity += amountToAdd;
                    amount -= amountToAdd;

                    if (amount <= 0)
                    {
                        OnInventoryChanged?.Invoke();
                        return true;
                    }
                }
            }
        }

        // ถ้ายังเหลือของ ให้หาช่องว่างใส่
        foreach (var slot in slots)
        {
            if (slot.IsEmpty())
            {
                int amountToAdd = item.isStackable ? Mathf.Min(amount, item.maxStackAmount) : 1;
                slot.item = item;
                slot.quantity = amountToAdd;
                amount -= amountToAdd;

                if (amount <= 0)
                {
                    OnInventoryChanged?.Invoke();
                    return true;
                }
            }
        }

        OnInventoryChanged?.Invoke();
        return amount <= 0; // false ถ้ากระเป๋าเต็มและของยังเหลือ

        OnInventoryChanged?.Invoke();
        Debug.Log("OnInventoryChanged Invoked! Listeners: " + OnInventoryChanged.GetPersistentEventCount());
        return amount <= 0;
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