using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SlotUI : MonoBehaviour, IPointerClickHandler
{
    public Image iconImage;
    public TextMeshProUGUI quantityText;
    public TextMeshProUGUI itemNameText;

    private InventorySlot currentSlot;
    private int slotIndex;

    private void Awake()
    {
        Button btn = GetComponent<Button>();
        if (btn != null) Destroy(btn);

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

    public void Init(int index)
    {
        slotIndex = index;
    }

    public void SetSlot(InventorySlot slot)
    {
        currentSlot = slot;

        if (iconImage == null) return;

        if (slot == null || slot.IsEmpty())
        {
            ClearSlot();
            return;
        }

        if (slot.item.icon != null)
        {
            iconImage.sprite = slot.item.icon;
            iconImage.color = Color.white;
        }

        if (itemNameText != null)
        {
            itemNameText.text = slot.item.itemName;
            itemNameText.gameObject.SetActive(true);
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData == null) return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (Hotbar.Instance == null) return;
            Hotbar.Instance.MoveFromInventory(slotIndex);
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (currentSlot == null || currentSlot.IsEmpty()) return;
            if (ItemDetailPanel.Instance == null) return;
            ItemDetailPanel.Instance.ShowDetail(currentSlot.item);
        }
    }

    public void ClearSlot()
    {
        currentSlot = null;
        if (iconImage != null) iconImage.color = new Color(1, 1, 1, 0);
        if (quantityText != null) { quantityText.text = ""; quantityText.gameObject.SetActive(false); }
        if (itemNameText != null) { itemNameText.text = ""; itemNameText.gameObject.SetActive(false); }
    }
}