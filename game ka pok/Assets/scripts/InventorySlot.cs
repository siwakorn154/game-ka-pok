[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int quantity;

    public InventorySlot()
    {
        item = null;
        quantity = 0;
    }

    public bool IsEmpty()
    {
        return item == null;
    }

    public void Clear()
    {
        item = null;
        quantity = 0;
    }
}
