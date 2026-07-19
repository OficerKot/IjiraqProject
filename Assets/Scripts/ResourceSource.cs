using System;
using UnityEngine;

/// <summary>
/// Источник ресурсов. В отличие от других объектов может быть разрушен постановкой соответствующего ему домино.
/// </summary>
public class ResourceSource : MonoBehaviour, Interactable, ICellContent
{
    [SerializeField] public ItemData resource;
    [SerializeField] ToolType toolToDestroy;
    [SerializeField] public Cell curCell;

    /// <summary>
    /// Подбирает ресурс, добавляет его в инвентарь и уничтожает объект.
    /// </summary>
    public virtual void Break()
    {
        if (resource)
        {
            Inventory.Instance.AddItem(resource);
        }
        curCell.SetCurContent(null);
        Destroy(gameObject);
    }

    /// <summary>
    /// Размещает источник в указанной клетке.
    /// </summary>
    /// <param name="cell">Клетка для размещения ресурса.</param>
    public virtual void PutInCell(Cell cell)
    {
        curCell = cell;
        PutInCell();
    }

    /// <summary>
    /// Проверяет, можно ли сломать источник с помощью указанных частей домино.
    /// </summary>
    /// <param name="cur">Первая часть домино.</param>
    /// <param name="other">Вторая часть домино.</param>
    /// <returns>True если есть подходящий инструмент или любой инструмент подходит.</returns>
    public virtual bool CanBeBrokenBy(DominoPart cur, DominoPart other)
    {
        return cur.data.characteristics.tool == toolToDestroy || other.data.characteristics.tool == toolToDestroy || toolToDestroy == ToolType.Any;
    }

    /// <summary>
    /// Размещает источник в текущей клетке и устанавливает её свойства.
    /// </summary>
    public virtual void PutInCell()
    {
        curCell.SetCurItem(ItemManager.Instance.GetItemByID(resource.));
        transform.position = curCell.transform.position;
        transform.Translate(0, 0, -curCell.transform.position.z);
    }
}