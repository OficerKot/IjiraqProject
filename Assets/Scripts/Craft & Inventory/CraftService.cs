using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;


public class CraftService : ICraftService
{
    private HashSet<ItemData> _availableItems = new HashSet<ItemData>();
    private HashSet<ItemData> _exploredRecipes = new HashSet<ItemData>();

    public event Action OnRecipesUpdated;

    private ItemsDataBase _itemsDB;
    private IInventory _inventory;
    

    [Inject]
    private void Construct(IInventory inventory, ItemsDataBase itemsDB)
    {
        _itemsDB = itemsDB;
        _inventory = inventory;
    }

    /// <summary>
    /// Добавляет новый рецепт
    /// </summary>
    void ExploreNewRecipe(ItemData item)
    {
        _exploredRecipes.Add(item);
    }

    /// <summary>
    /// Создает предмет и удаляет из инвентаря необходимые для крафта предметы.
    /// </summary>
    /// <param name="obj">Данные предмета для создания.</param>
    public void Craft(ItemData obj)
    {
        if (CanCraft(obj))
        {
            foreach (ItemData i in obj.itemsForCraft)
            {
                _inventory.RemoveItem(i);
            }

            // это тоже вынести 
            GameObject spawnedObj = GameObject.Instantiate(obj.prefab);
            spawnedObj.GetComponent<Item>().PickAndDestroy();
            // где добавление в инвентарь..
            UpdateRecipes();
        }
        else Debug.Log("You don't have all items to craft it!");
    }

    /// <summary>
    /// Обновляет доступные рецепты на основе текущего состояния инвентаря.
    /// </summary>
    void UpdateRecipes()
    {
        _availableItems.Clear();

        HashSet<ItemData> itemsInInventory = new HashSet<ItemData>(_inventory.GetCurItems().Keys);
    
        foreach (ItemData item in _itemsDB.allItems)
        {
            if (item.craftSet.IsSubsetOf(itemsInInventory))
            {
                _availableItems.Add(item);
            }
        }

        OnRecipesUpdated.Invoke();
    }

    public bool CanCraft(ItemData item)
    {
        return _availableItems.Contains(item);
    }

    public bool IsExplored(ItemData item)
    {
        return _exploredRecipes.Contains(item);
    }
}
