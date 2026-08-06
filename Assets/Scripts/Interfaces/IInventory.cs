using System;
using System.Collections.Generic;

public interface IInventory
{

    void AddItems(ItemData resource, int count);
    void AddItem(ItemData resource);
    void RemoveItem(ItemData data);
    bool Contains(ItemData resource);
    int Count(ItemData resource);
    bool IsFull();
    Dictionary<ItemData, int> GetCurItems();

    public event Action<ItemData> OnItemAdded;
    public event Action<ItemData> OnItemRemoved;
    public event Action OnInventoryFull;
    public event Action OnInventoryChanged;
}