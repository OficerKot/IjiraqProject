using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Данные предмета
/// </summary>
[CreateAssetMenu(fileName = "New item", menuName = "Items/ItemData")]
public class ItemData : ScriptableObject
{
    public string ID { get; private set; }
    public GameObject prefab;
    public GameObject UIprefab;

    public ToolType toolToDestroy {  get; private set; }
    public ObjectType type { get; private set; } = ObjectType.Any;

    public ItemData[] itemsForCraft;
    public HashSet<ItemData> craftSet = new HashSet<ItemData>();

    private void OnEnable() 
    {
        if (itemsForCraft.Length != 0)
        {
            foreach (ItemData item in itemsForCraft)
            {
                craftSet.Add(item);
            }
        }
    }
}