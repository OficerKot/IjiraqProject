using System.Collections.Generic;
using UnityEngine;
using VContainer;
using UnityEngine.UI;

public class UIInventory : PauseBehaviour
{
    [SerializeField] GameObject inventory;
    public UIInventoryCell[] cellsList;
    Dictionary<UIInventoryCell, ItemData> cells = new Dictionary<UIInventoryCell, ItemData>();

    [SerializeField] Button inventoryButton;

    bool isOpened = false;
    bool isActive = true;

    IInventory _inventory;
    [Inject]
    void Construct(IInventory inventory)
    {
        _inventory = inventory;
        
        _inventory.OnItemAdded += AddItem;
        _inventory.OnItemRemoved += RemoveItem;

    }

    void Start()
    {
        for (int i = 0; i < Inventory.MAX_SIZE; i++)
        {
            cells.Add(cellsList[i], null);
        }

        inventoryButton.onClick.AddListener(Interact);
    }

    void AddItem(ItemData item)
    {
      
        if(_inventory.Count(item) == 1)
        {
            AddIcon(item);
            Debug.Log("Added Icon");
        }
        else
        {
            IncreaseCounter(item);
            Debug.Log("Increased");

        }
    }

    void IncreaseCounter(ItemData item)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            UIInventoryCell cell = cellsList[i];
            if (cells[cell] == item)
            {
                cell.AddToCounter(1);
                break;
            }
        }
    }
    void AddIcon(ItemData item)
    {
        if (!isOpened) inventory.SetActive(true);
        for (int i = 0; i < cells.Count; i++)
        {
            UIInventoryCell cell = cellsList[i];
            if (cells[cell]) continue;
            else
            {
                cells[cell] = item;
                cell.PutItem(Instantiate(item.UIprefab, inventory.transform));
                break;
            }
        }

        if (!isOpened) inventory.SetActive(false);
    }


    void RemoveItem(ItemData item)
    {
        if(_inventory.Count(item) == 0)
        {
            RemoveIcon(item);
        }
        else
        {
            DecreaseCounter(item);
        }
    }

    void DecreaseCounter(ItemData item)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            UIInventoryCell cell = cellsList[i];
            if (cells[cell] == item)
            {
                cell.AddToCounter(-1);
            }
        }
    }
    void RemoveIcon(ItemData item)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            UIInventoryCell cell = cellsList[i];
            if (cells[cell] == item)
            {
                cells[cell] = null;
                cell.RemoveItem();
            }
        }
    }

    void Interact()
    {
        if (!isActive) return;

        if (!isOpened)
        {
            isOpened = true;
            Open();
        }
        else
        {
            isOpened = false;
            Close();
        }
    }

    void Open()
    {
        inventory.SetActive(true);
    }

    void Close()
    {
        inventory.SetActive(false);
    }

    public override void OnGamePaused(bool isGamePaused)
    {
        isActive = !isGamePaused;
    }
}
