using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public Transform slotParent;
    public GameObject slotPrefab;
    public GameObject inventoryPanel;

    private List<SlotUI> slotUIs = new List<SlotUI>();

    private void Start()
{
    for (int i = 0; i < inventory.slots.Count; i++)
    {
        GameObject slotObj = Instantiate(slotPrefab, slotParent);
        SlotUI slotUI = slotObj.GetComponent<SlotUI>();
        slotUI.Init(i); // ← ส่ง index ไปด้วย
        slotUIs.Add(slotUI);
    }

    inventory.OnInventoryChanged.AddListener(UpdateUI);
    inventoryPanel.SetActive(false);
}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("กด E แล้ว!");
            bool isOpen = !inventoryPanel.activeSelf;
            inventoryPanel.SetActive(isOpen);
            if (isOpen) UpdateUI();
        }
    }

    private void UpdateUI()
{
    for (int i = 0; i < slotUIs.Count; i++)
    {
        if (slotUIs[i] == null)
        {
            Debug.LogError($"slotUI[{i}] เป็น null!");
            continue;
        }
        slotUIs[i].SetSlot(inventory.slots[i]);
    }
    Debug.Log("UpdateUI เรียกแล้ว จำนวน slot: " + slotUIs.Count);
}
}