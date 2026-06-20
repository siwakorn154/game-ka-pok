using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HotbarSlotUI : MonoBehaviour, IPointerClickHandler
{
    public Image background;
    public Image iconImage;
    public TextMeshProUGUI quantityText;
    public TextMeshProUGUI numberText;
    public Image selectedHighlight;

    public Color normalColor = new Color(0.3f, 0.3f, 0.3f, 0.9f);
    public Color selectedColor = new Color(1f, 0.85f, 0f, 1f);

    private int slotIndex;
    private InventorySlot currentSlot;

    public void Init(int index)
    {
        slotIndex = index;

        // แสดงเลข 1-9 และ 0
        if (numberText != null)
            numberText.text = index < 9 ? (index + 1).ToString() : "0";

        if (iconImage != null)
            iconImage.color = new Color(1, 1, 1, 0);

        if (quantityText != null)
            quantityText.gameObject.SetActive(false);

        SetSelected(false);
    }

    public void SetSlot(InventorySlot slot)
    {
        currentSlot = slot;

        if (slot == null || slot.IsEmpty())
        {
            if (iconImage != null)
                iconImage.color = new Color(1, 1, 1, 0);
            if (quantityText != null)
                quantityText.gameObject.SetActive(false);
            return;
        }

        if (iconImage != null && slot.item.icon != null)
        {
            iconImage.sprite = slot.item.icon;
            iconImage.color = Color.white;
        }

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

    public void SetSelected(bool selected)
    {
        if (selectedHighlight != null)
            selectedHighlight.gameObject.SetActive(selected);
        if (background != null)
            background.color = selected ? selectedColor : normalColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            Hotbar.Instance.SetSelected(slotIndex);

        if (eventData.button == PointerEventData.InputButton.Right)
            Hotbar.Instance.MoveToInventory(slotIndex);
    }
}