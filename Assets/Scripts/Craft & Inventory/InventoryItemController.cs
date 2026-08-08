using System;
using UnityEngine;
using VContainer;

public class InventoryItemController
{
    Item _spawnedItem;

    ItemFactory _itemFactory;
    HandManager _handManager;
    IInventory _inventory;
    [Inject]
    void Construct(ItemFactory itemFactory, HandManager handManager, IInventory inventory)
    {
        _itemFactory = itemFactory;
        _handManager = handManager;
        _inventory = inventory;
    }

    public ToggleResult ToggleItem(ItemData data)
    {
        if (_handManager.IsHandFree())
        {
            _spawnedItem = _itemFactory.CreateItem(data);
            _spawnedItem.OnItemPlaced += OnItemPlaced;

            _handManager.Take(_spawnedItem.gameObject);

            return ToggleResult.Taken;
        }

        if (_handManager.IsHoldingItem(data))
        {
            _handManager.Release();
            UnityEngine.Object.Destroy(_spawnedItem.gameObject);

            return ToggleResult.Released;
        }

        return ToggleResult.Failed;
    }

    void OnItemPlaced()
    {
        _handManager.Release();
        _inventory.RemoveItem(_spawnedItem.data);

        _spawnedItem = null;
    }
}

public enum ToggleResult
{
    Taken, Released, Failed
}
