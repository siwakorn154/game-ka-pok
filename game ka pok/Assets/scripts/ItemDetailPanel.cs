using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetailPanel : MonoBehaviour
{
    public static ItemDetailPanel Instance;

    [Header("References")]
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescText;
    public Button closeButton;

    [Header("Layout")]
    public RectTransform inventoryPanel;       // ลาก InventoryPanel มาใส่
    public RectTransform detailPanel;          // ลาก ItemDetailPanel มาใส่ (ตัวเอง)

    private Vector2 inventoryCenterPos;        // ตำแหน่งกลางของ inventory
    private Vector2 inventoryShiftedPos;       // ตำแหน่งเมื่อเลื่อนซ้าย

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // จำตำแหน่งเดิมของ inventory
        inventoryCenterPos = inventoryPanel.anchoredPosition;
        inventoryShiftedPos = inventoryCenterPos + new Vector2(-220, 0); // เลื่อนซ้าย 220

        closeButton.onClick.AddListener(HideDetail);

        detailPanel.gameObject.SetActive(false);
    }

    public void ShowDetail(ItemData item)
    {
        if (item == null) return;

        // ใส่ข้อมูลไอเทม
        itemNameText.text = item.itemName;
        itemDescText.text = item.description;

        if (item.icon != null)
        {
            itemIcon.sprite = item.icon;
            itemIcon.color = Color.white;
        }
        else
        {
            itemIcon.color = new Color(1, 1, 1, 0);
        }

        // เลื่อน inventory ไปซ้าย แล้วเปิด detail
        inventoryPanel.anchoredPosition = inventoryShiftedPos;
        detailPanel.gameObject.SetActive(true);
    }

    public void HideDetail()
    {
        // ปิด detail แล้วเลื่อน inventory กลับกลาง
        detailPanel.gameObject.SetActive(false);
        inventoryPanel.anchoredPosition = inventoryCenterPos;
    }
}