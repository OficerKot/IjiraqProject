using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using System;
using VContainer;

/// <summary>
/// Основной класс управления домино, обрабатывающий перетаскивание, размещение и взаимодействие с клетками.
/// </summary>
public class Domino : PauseBehaviour, ILayerSortable
{
    public DominoPart part1 { get; private set; }
    public DominoPart part2 { get; private set; }

    public Cell curCell1 { get; private set; }
    public Cell curCell2 { get; private set; }

    public GameObject pivot;
    Vector3 part1Pos, part2Pos;

    bool isBeingGrabbed = false;

    public event Action<Domino> OnPlaced;
    public static event Action<Domino> OnAnyDominoPlaced;

    #region Sort
    public event Action<ILayerSortable> Picked;
    public event Action<ILayerSortable> Placed;

    public SortingOrder defaultSortingOrder { get; } = SortingOrder.item;
    public SortingGroup sortingGroup => GetComponent<SortingGroup>();

    LayerSorter _layerSorter;
    private HandManager _handManager;
    #endregion


    [Inject]
    public void Construct(HandManager handManager, LayerSorter layerSorter, DominoPart p1, DominoPart p2)
    {
        _handManager = handManager;
        _layerSorter = layerSorter;

        part1 = p1;
        part2 = p2;

        part1.transform.SetParent(transform);
        part2.transform.SetParent(transform);

        SetPartsPositions();
        SpawnAndSetPivot();
    }

    /// <summary>
    /// Расчёт и установка расположения частей домино
    /// </summary>
    void SetPartsPositions()
    {
        var spriteBounds = GetComponent<SpriteRenderer>().bounds;
        var height = spriteBounds.size.y;

        part1Pos = new Vector3(0, height / 4, 0);
        part2Pos = new Vector3(0, -height / 4, 0);

        part1.transform.localPosition = part1Pos;
        part2.transform.localPosition = part2Pos;
    }

    /// <summary>
    /// Создает точку вращения (pivot) для домино.
    /// </summary>
    void SpawnAndSetPivot()
    {
        Vector2 centerPosition = GetComponent<SpriteRenderer>().bounds.center;

        pivot = new GameObject("Pivot");
      
        pivot.transform.position = centerPosition;
        pivot.transform.rotation = Quaternion.identity;

        transform.SetParent(pivot.transform);
    }

    /// <summary>
    /// Поднимает домино для перетаскивания.
    /// </summary>
    public void PickUp()
    {
        isBeingGrabbed = true;

        if (curCell1 && curCell2)
        {
            part1.ClearAllNeighbors();
            part2.ClearAllNeighbors();

            ClearCellData();
        }
    }

    private void OnDestroy()
    {
        ClearCellData();
        _layerSorter?.Unregister(this);
    }

    /// <summary>
    /// Проверяет, размещено ли домино на поле.
    /// </summary>
    /// <returns>True если домино размещено, false если находится в руке.</returns>
    public bool isPlaced()
    {
        return !isBeingGrabbed;
    }

    private void FixedUpdate()
    {
        if (isBeingGrabbed)
        {
            Interact();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isBeingGrabbed && collision.gameObject.layer == 6 && !EventSystem.current.IsPointerOverGameObject())
        {
            if (!collision.GetComponent<Cell>().IsFreeForDomino())
            {
                return;
            }

            ClearCellData();
            curCell1 = collision.GetComponent<Cell>();
            Cell minDistCell = FindMinDistCell();

            if (minDistCell == null)
            {
                return;
            }
            else
            {
                curCell2 = minDistCell;
                curCell1.Highlight();
                curCell2.Highlight();
                return;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isBeingGrabbed) ClearCellData();
    }

    /// <summary>
    /// Находит ближайшую подходящую клетку для размещения второй части домино.
    /// </summary>
    Cell FindMinDistCell()
    {
        float minDist = float.MaxValue;
        Cell minDistCell = null;
        foreach (Cell nearCell in curCell1.neighbourCells)
        {
            if (nearCell && CellsAreOK(curCell1, nearCell))
            {
                if (minDist > GetDistance2(pivot.transform, nearCell.transform))
                {
                    minDist = GetDistance2(pivot.transform, nearCell.transform);
                    minDistCell = nearCell;
                }
            }
        }
        return minDistCell;
    }

    /// <summary>
    /// Проверяет, подходит ли клетка для размещения домино.
    /// </summary>
    bool CellsAreOK(Cell cell1, Cell cell2)
    {
        if (!IsSameRotationAngle(cell2.transform.position, curCell1.transform.position)) return false;
        if (!DominoPlacementValidator.ValidatePlacement(this, cell1, cell2)) return false;

        return true;
    }

    /// <summary>
    /// Очищает данные о текущих клетках и снимает выделение.
    /// </summary>
    void ClearCellData() // по моему клетка сама должна это делать
    {
        if (curCell1)
        {
            curCell1.NoHighlight();
            curCell1 = null;
        }
        if (curCell2)
        {
            curCell2.NoHighlight();
            curCell2 = null;
        }

        part1.ChangeIsBeingPlacedFlag(false);
        part2.ChangeIsBeingPlacedFlag(false);
    }

    /// <summary>
    /// Вычисляет квадрат расстояния между двумя трансформами.
    /// </summary>
    float GetDistance2(Transform pos1, Transform pos2)
    {
        float dist = Mathf.Pow(pos1.position.x - pos2.position.x, 2) + Mathf.Pow(pos1.position.y - pos2.position.y, 2);
        return dist;
    }

    /// <summary>
    /// Размещает домино в выбранных клетках.
    /// </summary>
    void PutInTheCells() // ЧТО ЭТО
    {
        isBeingGrabbed = false;
        OnPlaced.Invoke(this);
        OnAnyDominoPlaced.Invoke(this);

        _handManager.Take(null);

        curCell1.GetCurContent()?.PickAndDestroy();
        curCell2.GetCurContent()?.PickAndDestroy();

        Collider2D collider1 = curCell1.GetComponent<BoxCollider2D>();
        Collider2D collider2 = curCell2.GetComponent<BoxCollider2D>();

        TeleportToCells(collider1.transform, collider2.transform);
        AddToCells(part1, part2);

        AudioManager.Play(SoundType.BonePlace);

    }

    /// <summary>
    /// Привязывает части домино к соответствующим клеткам.
    /// </summary>
    
    void AddToCells(DominoPart d1, DominoPart d2)
    {
        if (GetDistance2(d1.transform, curCell1.transform) > GetDistance2(d2.transform, curCell1.transform))
        {
            curCell1.SetCurDomino(d2);
            curCell2.SetCurDomino(d1);
        }
        else
        {
            curCell1.SetCurDomino(d1);
            curCell2.SetCurDomino(d2);
        }
    }

    /// <summary>
    /// Телепортирует домино в центр между двумя клетками.
    /// </summary>
    void TeleportToCells(Transform pos1, Transform pos2)
    {
        Vector2 targetPos = (pos1.position + pos2.transform.position) / 2f;
        pivot.transform.position = targetPos;
    }


    /// <summary>
    /// Проверяет совпадение направления домино с направлением 2х клеток.
    /// </summary>
    bool IsSameRotationAngle(Vector3 pos1, Vector3 pos2)
    {
        bool cellsAreHorizontal = Mathf.Abs(pos1.x - pos2.x) > Mathf.Abs(pos1.y - pos2.y);
        float dominoAngle = NormalizeAngle(pivot.transform.eulerAngles.z);
        bool dominoIsHorizontal = (dominoAngle >= 45f && dominoAngle <= 135f);
        return cellsAreHorizontal == dominoIsHorizontal;
    }

    /// <summary>
    /// Нормализует угол в диапазон 0-180 градусов.
    /// </summary>
    float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0) angle += 360f;

        if (angle > 180f) angle = 360f - angle;
        return angle;
    }

    /// <summary>
    /// Обрабатывает взаимодействие с домино (перемещение, вращение, размещение).
    /// </summary>
    void Interact()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.A))
        {
            Rotate(90);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Rotate(-90);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (curCell1 && curCell2 && !EventSystem.current.IsPointerOverGameObject())
            {
                curCell1.NoHighlight();
                curCell2.NoHighlight();
                PutInTheCells();
            }
        }
    }

    /// <summary>
    /// Вращает домино на указанный угол.
    /// </summary>
    /// <param name="degree">Угол вращения в градусах.</param>
    void Rotate(float degree = 90)
    {
        Debug.Log($"BEFORE Pivot: {pivot.transform.eulerAngles.z}");
        Debug.Log($"BEFORE Domino world: {transform.eulerAngles.z}");
        Debug.Log($"BEFORE Domino local: {transform.localEulerAngles.z}");

        pivot.transform.Rotate(0, 0, degree);

        Debug.Log($"AFTER Pivot: {pivot.transform.eulerAngles.z}");
        Debug.Log($"AFTER Domino world: {transform.eulerAngles.z}");
        Debug.Log($"AFTER Domino local: {transform.localEulerAngles.z}");
    }

    /// <summary>
    /// Перемещает домино к позиции курсора мыши.
    /// </summary>
    void Move()
    {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = 0;

        pivot.transform.position = Vector3.Lerp(pivot.transform.position, targetPos, 1);
    }

}