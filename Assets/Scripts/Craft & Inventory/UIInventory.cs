using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;
using static UnityEditor.Progress;

public class UIInventory : PauseBehaviour
{
    [SerializeField] GameObject inventory;
    public UIInventoryCell[] cellsList;
    Dictionary<UIInventoryCell, ItemData> cells = new Dictionary<UIInventoryCell, ItemData>();

    [SerializeField] Button inventoryButton;

    bool isOpened = false;
    bool isActive = true;

    IInventory _inventory;
    UIConfig _config;
    InventoryItemController _itemController;
    [Inject]
    void Construct(IInventory inventory,UIConfig config, InventoryItemController controller)
    {
        _inventory = inventory;
        _config = config;
        _itemController = controller;
        
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

    void AddItem(ItemData item, int curCnt)
    {
      
        if(curCnt == 1)
        {
            AddIcon(item);
        }
        else
        {
            UpdateCell(item, curCnt);
        }
    }

    void RemoveItem(ItemData item, int curCnt)
    {
        if (curCnt == 0)
        {
            RemoveIcon(item);
        }
        else
        {
            UpdateCell(item, curCnt);
        }
    }

    void UpdateCell(ItemData item, int cnt)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            UIInventoryCell cell = cellsList[i];
            if (cells[cell] == item)
            {
                cell.SetCounter(cnt);
                cell.icon.SetIconState(IconState.Released);
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
                ItemIcon icon = CreateAndInitIcon(item);
                icon.OnClick += OnIconClick;

                cell.PutIcon(icon);
                break;
            }
        }

        if (!isOpened) inventory.SetActive(false);
    }

    void OnIconClick(ItemIcon icon)
    {
        ToggleResult res = _itemController.ToggleItem(icon.data);
        if (res == ToggleResult.Taken)
        {
            icon.SetIconState(IconState.Pressed);
            return;
        }

        if (res == ToggleResult.Released)
        {
            icon.SetIconState(IconState.Released);
            return;
        }

    }
    ItemIcon CreateAndInitIcon(ItemData item)
    {
        ItemIcon icon = Instantiate(_config.itemIconPrefab, inventory.transform).GetComponent<ItemIcon>();
        icon.Init(item);

        return icon;
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
            Open();
        }
        else
        {
            Close();
        }
    }

    void Open()
    {
        isOpened = true;
        inventory.SetActive(true);
    }

    void Close()
    {
        isOpened = false;
        inventory.SetActive(false);
    }

    public override void OnGamePaused(bool isGamePaused)
    {
        isActive = !isGamePaused;
    }
}
