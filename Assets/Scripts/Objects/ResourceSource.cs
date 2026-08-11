using System;
using UnityEngine;
using VContainer;

/// <summary>
/// Источник ресурсов. В отличие от других объектов может быть разрушен постановкой соответствующего ему домино.
/// </summary>
public class ResourceSource : MonoBehaviour, IInteractable, ICellContent
{
    [SerializeField] public ItemData resource;
    [SerializeField] ObjectType type;
    [SerializeField] ToolType toolToDestroy;
    [SerializeField] bool isClickable;
    [SerializeField] public Cell curCell;

    private IInventory _inventory;
    public event Action Destroyed;

    [Inject]
    private void Construct(IInventory inventory)
    {
        _inventory = inventory;
    }

    private void OnMouseDown()
    {
        if (isClickable)
        {
            OnClick();
        }
    }

    protected void Destroy()
    {
        Destroyed?.Invoke();
        Destroy(gameObject);
    }
    public virtual void OnClick()
    {
        Pick();
    }

    /// <summary>
    /// Подбирает ресурс, добавляет его в инвентарь и уничтожает объект.
    /// </summary>
    public virtual void PickAndDestroy()
    {
        Pick();
        Destroyed?.Invoke();
        Destroy(gameObject);
    }

    protected virtual void Pick()
    {
        if (resource)
        {
            _inventory.AddItem(resource);
        }
    }

    /// <summary>
    /// Проверяет, можно ли сломать источник с помощью переданного домино.
    /// </summary>
    /// <param name="d">Домино.</param>
    /// <returns>True если есть подходящий инструмент или любой инструмент подходит.</returns>

    public virtual bool CanBeBrokenBy(Domino d)
    {
        DominoPart p1 = d.part1;
        DominoPart p2 = d.part2;
        return p1.sigilVariantData.sigilTypeData.characteristics.tool == toolToDestroy || p2.sigilVariantData.sigilTypeData.characteristics.tool == toolToDestroy || toolToDestroy == ToolType.Any;
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
    /// Размещает источник в текущей клетке и устанавливает её свойства.
    /// </summary>
    public virtual void PutInCell()
    {
        curCell.SetCurContent(this);
        transform.position = curCell.transform.position;
        transform.Translate(0, 0, -curCell.transform.position.z);
    }

    ObjectType ICellContent.GetType()
    {
        return type;
    }
}