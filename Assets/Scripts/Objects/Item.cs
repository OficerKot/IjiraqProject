using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

/// <summary>
/// Для тех объектов, которые могут взаимодействовать с клетками
/// </summary>
public interface IInteractable
{
    public void PutInCell(Cell cell);
    public void PutInCell();
}

/// <summary>
/// Класс для объектов, которые могут находиться в инвентаре, подбираться с поля и размещаться на поле с определёнными условиями
/// </summary>
public class Item : PauseBehaviour, IInteractable, ICellContent
{
    [field: SerializeField] public ItemData data { get; private set; }
    [SerializeField] Cell curCell;
    bool isPlaced = true;
    public event Action OnItemPlaced;

    private IInventory _inventory;

    [Inject]
    private void Construct(IInventory inventory)
    {
        _inventory = inventory;
    }

    public virtual void OnMouseDown()
    {
        if (isPlaced && (_inventory.Contains(ItemsDataBase.Instance.GetItemByID(data.ID)) || !_inventory.IsFull()))
        {
            Remove();
        }

        else if (Input.GetKeyDown(KeyCode.Mouse0) && curCell)
        {
            curCell.NoHighlight();
            PutInCell();
        }

    }
    private void OnDestroy()
    {
        if(curCell)
        {
            curCell.SetFree();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Cell cell = collision.GetComponent<Cell>();
        if (!isPlaced && collision.gameObject.layer == 6 && CanPutInCell(cell))
        {
            CheckCells(cell);
        }
    }
    public virtual void CheckCells(Cell cell)
    {
        if (curCell) return;

        curCell = cell;
        curCell.Highlight();
        return;
    }

    public virtual void TurnOffHighlightedCells()
    {
        if (curCell && !isPlaced)
        {
            curCell.NoHighlight();
            curCell = null;
        }
    }

    /// <summary>
    /// Проверяет, можно ли поместить объект в указанную клетку.
    /// Базовая реализация разрешает установку только в свободную клетку.
    /// Может быть переопределён в наследниках для дополнительных условий.
    /// </summary>
    /// <param name="c">Клетка, в которую пытаются установить объект.</param>
    /// <returns>
    /// True — если объект можно установить в эту клетку,  
    /// False — если установка запрещена.
    /// </returns>
    public virtual bool CanPutInCell(Cell c)
    {
        return c.IsFree(); 
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        TurnOffHighlightedCells();
    }
    private void Update()
    {
        if (!isPlaced) Move();
    }
    public bool GetIsPlaced()
    {
        return isPlaced;
    }
    public void SetIsPlaced(bool val)
    {
        isPlaced = val;
    }
    public string GetID()
    {
        return data.ID;
    }
  
    public void PutInCell(Cell cell)
    {
        curCell = cell;
        PutInCell();
    }
    /// <summary>
    /// Установка предмета в клетку/множество клеток
    /// </summary>
    public virtual void PutInCell()
    {
        curCell.SetCurContent(this);
        isPlaced = true;
        InvokeAction();
        transform.position = curCell.transform.position;
        transform.Translate(0, 0, -curCell.transform.position.z);

        _inventory.RemoveItem(data);
        HandManager.Instance?.PutInHand(null);
    }

    protected void InvokeAction()
    {
        OnItemPlaced?.Invoke();
    }
    void Move() 
    {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = 0;
        transform.position = Vector3.Lerp(transform.position, targetPos, 1);
    }

    ObjectType ICellContent.GetType()
    {
        return data.type;
    }

    public virtual bool CanBeBrokenBy(Domino d)
    {
        DominoPart p1 = d.part1;
        DominoPart p2 = d.part2;
        return p1.sigilVariantData.sigilTypeData.characteristics.tool == data.toolToDestroy || p2.sigilVariantData.sigilTypeData.characteristics.tool == data.toolToDestroy || data.toolToDestroy == ToolType.Any;
    }

    /// <summary>
    /// Сбор предмета в инвентарь
    /// </summary>
    public void Remove()
    {
        if (_inventory.Contains(ItemsDataBase.Instance.GetItemByID(data.ID)) || !_inventory.IsFull())
        {
            _inventory.AddItem(ItemsDataBase.Instance.GetItemByID(data.ID));
            Destroy(gameObject);
        }
    }
  
}
