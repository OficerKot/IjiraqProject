using System.Collections.Generic;

/// <summary>
/// Инвентарь игрока.
/// Хранит предметы по ID и их количество,
/// ограничен максимальным размером и автоматически обновляет UI.
/// </summary>
public class Inventory : IInventory
{
    public const int MAX_SIZE = 6;
    Dictionary<ItemData, int> items = new Dictionary<ItemData, int>();

    /// <summary>
    /// Добавление предмета в ивентарь
    /// </summary>
    /// <param name="i">Данные добавляемого предмета.</param>
    public void AddItem(ItemData i)
    {
        if (Contains(i) || items.Count < MAX_SIZE)
        {
            AudioManager.Play(SoundType.Pickup); // убрать
            if (!items.ContainsKey(i))
            {
                items.Add(i, 0);
                UIInventory.Instance.AddNewItem(i);
            }
            items[i]++;
            UIInventory.Instance.AddOneMoreItem(i);
            // UICraftWindow.Instance.CheckInventory(i);
        }
        else
        {
            AudioManager.Play(SoundType.FullInventory); // убрать..
        }
    }
    /// <summary>
    /// Добавление нескольких предметов в инвентарь
    /// </summary>
    /// <param name="i">Данные предмета</param>
    /// <param name="count">Количество</param>
    public void AddItems(ItemData i, int count)
    {
        if (Contains(i) || items.Count < MAX_SIZE)
        {
            AudioManager.Play(SoundType.Pickup); // убрать

            if (!items.ContainsKey(i))
            {
                items.Add(i, 0);
                UIInventory.Instance.AddNewItem(i);
            }
            for (int j = 0; j < count; j++)
            {
                items[i]++;
                UIInventory.Instance.AddOneMoreItem(i);
            }
            //UICraftWindow.Instance.CheckInventory(i);
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
            UIInventory.Instance.RemoveOneItem(i);

            if (items[i] < 1)
            {
                UIInventory.Instance.RemoveItemIcon(i);
                items.Remove(i);
            }
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
    /// <returns>Словарь ID предметов и их количества.</returns>
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
        if (items.Count >= MAX_SIZE) AudioManager.Play(SoundType.FullInventory); // убрать 
        return items.Count >= MAX_SIZE;
    }
}
