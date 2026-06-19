using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI quantityText;
    public TextMeshProUGUI itemNameText; // เพิ่มตรงนี้

    private void Awake()
    {
        if (iconImage != null)
            iconImage.color = new Color(1, 1, 1, 0);

        if (quantityText != null)
        {
            quantityText.text = "";
            quantityText.gameObject.SetActive(false);
        }

        if (itemNameText != null)
        {
            itemNameText.text = "";
            itemNameText.gameObject.SetActive(false);
        }
    }

    public void SetSlot(InventorySlot slot)
    {
        if (iconImage == null) return;

        if (slot == null || slot.IsEmpty())
        {
            ClearSlot();
            return;
        }

        // แสดงไอคอน
        if (slot.item.icon != null)
        {
            iconImage.sprite = slot.item.icon;
            iconImage.color = Color.white;
        }

        // แสดงชื่อ
        if (itemNameText != null)
        {
            itemNameText.text = slot.item.itemName;
            itemNameText.gameObject.SetActive(true);
        }

        // แสดงจำนวน
        if (quantityText != null)
        {
            if (slot.quantity > 1)
            {
                quantityText.text = slot.quantity.ToString();
                quantityText.gameObject.SetActive(true);
            }
            else
            {
                quantityText.gameObject.SetActive(false);
            }
        }
    }

    public void ClearSlot()
    {
        if (iconImage != null)
            iconImage.color = new Color(1, 1, 1, 0);

        if (quantityText != null)
        {
            quantityText.text = "";
            quantityText.gameObject.SetActive(false);
        }

        if (itemNameText != null)
        {
            itemNameText.text = "";
            itemNameText.gameObject.SetActive(false);
        }
    }
}