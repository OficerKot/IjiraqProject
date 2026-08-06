using System;
using System.Collections.Generic;

/// <summary>
/// Инвентарь игрока.
/// Хранит предметы по ID и их количество,
/// ограничен максимальным размером и автоматически обновляет UI.
/// </summary>
public class Inventory : IInventory
{
    public const int MAX_SIZE = 6;

    public event Action<ItemData> OnItemAdded;
    public event Action<ItemData> OnItemRemoved;
    public event Action OnInventoryFull;
    public event Action OnInventoryChanged;

    Dictionary<ItemData, int> items = new Dictionary<ItemData, int>();

    /// <summary>
    /// Добавление предмета в ивентарь
    /// </summary>
    /// <param name="i">Данные добавляемого предмета.</param>
    public void AddItem(ItemData i)
    {
        if (Contains(i) || items.Count < MAX_SIZE)
        {
            if (!items.ContainsKey(i))
            {
                items.Add(i, 0);
            }
            items[i]++;
            OnItemAdded?.Invoke(i);
            OnInventoryChanged?.Invoke();
            // UICraftWindow.Instance.CheckInventory(i);
        }
        else
        {
            OnInventoryFull?.Invoke();
        }
    }
    /// <summary>
    /// Добавление нескольких предметов в инвентарь
    /// </summary>
    /// <param name="item">Данные предмета</param>
    /// <param name="count">Количество</param>
    public void AddItems(ItemData item, int count)
    {
        for(int i = 0; i < count && items.Count < MAX_SIZE; i++)
        {
            AddItem(item);
        }
    }

    /// <summary>
    /// Удаление предмета из инвентаря
    /// </summary>
    /// <param name="i">Данные предмета</param>
    public void RemoveItem(ItemData i)
    {
        if (items.ContainsKey(i))
        {
            items[i]--;
            if (items[i] < 1)
            {
                items.Remove(i);
            }
            OnItemRemoved?.Invoke(i);
            OnInventoryChanged?.Invoke();
            //UICraftWindow.Instance.CheckInventory(i);
        }

    }

    /// <summary>
    /// Проверка на наличие предмета в инвентаре
    /// </summary>
    /// <param name="i">Данные предмета</param>
    /// <returns></returns>
    public bool Contains(ItemData i)
    {
        return items.ContainsKey(i);
    }

    /// <summary>
    /// Возвращает текущие предметы инвентаря.
    /// </summary>
    /// <returns>Словарь предметов и их количества.</returns>
    public Dictionary<ItemData, int> GetCurItems()
    {
        return items;
    }

    /// <summary>
    /// Проверяет наличие свободного места в инвентаре
    /// </summary>
    /// <returns></returns>
    public bool IsFull()
    {
        return items.Count >= MAX_SIZE;
    }

    public int Count(ItemData resource)
    {
        return items[resource];
    }
}
