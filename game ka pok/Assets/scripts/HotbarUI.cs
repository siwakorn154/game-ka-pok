using System.Collections.Generic;
using UnityEngine;

public class HotbarUI : MonoBehaviour
{
    public Hotbar hotbar;
    public Transform slotParent;
    public GameObject hotbarSlotPrefab;

    private List<HotbarSlotUI> slotUIs = new List<HotbarSlotUI>();

    private void Start()
    {
        for (int i = 0; i < hotbar.slots.Count; i++)
        {
            GameObject obj = Instantiate(hotbarSlotPrefab, slotParent);
            var slotUI = obj.GetComponent<HotbarSlotUI>();
            slotUI.Init(i);
            slotUIs.Add(slotUI);
        }

        hotbar.OnHotbarChanged.AddListener(UpdateUI);
        hotbar.OnSelectionChanged.AddListener(UpdateSelection);
        UpdateUI();
        UpdateSelection();
    }

    private void Update()
    {
        // Scroll wheel เปลี่ยน slot
        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        if (scroll > 0f) // scroll ขึ้น = ซ้าย
            hotbar.SetSelected(hotbar.selectedIndex - 1 < 0 ? 9 : hotbar.selectedIndex - 1);
        else if (scroll < 0f) // scroll ลง = ขวา
            hotbar.SetSelected((hotbar.selectedIndex + 1) % 10);

        // ปุ่ม 1-9 และ 0
        for (int i = 0; i < 9; i++)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                hotbar.SetSelected(i);
        if (Input.GetKeyDown(KeyCode.Alpha0))
            hotbar.SetSelected(9);
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slotUIs.Count; i++)
            slotUIs[i].SetSlot(hotbar.slots[i]);
    }

    private void UpdateSelection()
    {
        for (int i = 0; i < slotUIs.Count; i++)
            slotUIs[i].SetSelected(i == hotbar.selectedIndex);
    }
}