using System.Collections.Generic;

public interface IInventory
{
    void AddItems(ItemData resource, int count);
    void AddItem(ItemData resource);
    void RemoveItem(ItemData data);
    bool Contains(ItemData resource);
    bool IsFull();
    Dictionary<string, int> GetCurItems();
}