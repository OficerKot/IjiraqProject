using UnityEngine;

/// <summary>
/// Хранит в себе все предметы в игре
/// </summary>
[CreateAssetMenu(fileName = "Items", menuName = "ItemsDataBase")]
public class ItemsDataBase : ScriptableObject
{
    public ItemData[] allItems;

    private static ItemsDataBase _instance;
    public static ItemsDataBase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<ItemsDataBase>("ItemManager");

                if (_instance == null)
                {
                    _instance = CreateInstance<ItemsDataBase>();
                    Debug.LogWarning("Created new ItemManager instance. Consider creating it as an asset.");
                }
            }
            return _instance;
        }
    }

    private void OnEnable()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    /// <summary>
    /// Возвращает данные предмета по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор предмета.</param>
    /// <returns>Данные предмета или null, если предмет не найден.</returns>
    public ItemData GetItemByID(string id)
    {
        return System.Array.Find(allItems, item => item.ID == id);
    }
}