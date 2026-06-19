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
        if (inventoryPanel == null)
        {
            Debug.LogError("InventoryUI: ยังไม่ได้ลาก InventoryPanel มาใส่!");
            return;
        }

        for (int i = 0; i < inventory.slots.Count; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotParent);
            slotUIs.Add(slotObj.GetComponent<SlotUI>());
        }

        inventory.OnInventoryChanged.AddListener(UpdateUI);

        inventoryPanel.SetActive(false); // ปิดตอนเริ่ม
        Debug.Log("Inventory UI พร้อมใช้งาน กด B เพื่อเปิด");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("กด B แล้ว!");
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