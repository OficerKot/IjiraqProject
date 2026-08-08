using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Интерфейс для клетки игрового поля.
/// </summary>
public interface ICell
{
    public void Highlight();
    public void NoHighlight();
    public bool IsNeighbour(Cell obj);
    public DominoPart GetCurDomino();
    public void SetFree();
}

/// <summary>
/// Класс клетки игрового поля, управляющей размещением домино и объектов.
/// </summary>
public class Cell : MonoBehaviour, ICell
{

    [SerializeField] DominoPart curDomino;
    [SerializeField] ICellContent curContent;
    [SerializeField] public List<Cell> neighbourCells = new List<Cell>();

    //--- ВЫНЕСТИ -------------------------------
    public event Action<Cell> OnDuplicationAllowed;
    public int duplicationBlockers = 0;
    SpriteRenderer cellSprite;
    Color previousColor;
    // ------------------------------------------

    void Start()
    {
        CheckForDuplications();

        cellSprite = gameObject.GetComponent<SpriteRenderer>();
        previousColor = cellSprite.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6 && NotAngular(other.transform))
        {
            neighbourCells.Add(other.GetComponent<Cell>());
        }
    }

    public bool HasNeighbourDominos()
    {
        foreach (Cell cell in neighbourCells)
        {
            if (cell.GetCurDomino()) return true;
        }
        return false;
    }

    /// <summary>
    /// Возвращает текущее домино в клетке.
    /// </summary>
    public DominoPart GetCurDomino()
    {
        return curDomino;
    }

    /// <summary>
    /// Возвращает содержимое клетки.
    /// </summary>
    public ICellContent GetCurContent()
    {
        return curContent;
    }

    /// <summary>
    /// Устанавливает домино в клетку.
    /// </summary>
    /// <param name="domino">Домино для размещения.</param>
    public void SetCurDomino(DominoPart domino)
    {
        curDomino = domino;

        CheckForDuplications();
        ClearThePond();
    }

    /// <summary>
    /// Устанавливает предмет в клетку.
    /// </summary>
    /// <param name="i">Предмет для размещения.</param>
    public void SetCurContent(ICellContent i)
    {
        CheckForDuplications();
        if (i == null)
        {
            return;
        }
        curContent = i;
    }

    /// <summary>
    /// Освобождает клетку, удаляя домино и содержимое.
    /// </summary>
    public void SetFree()
    {
        curDomino = null;
        curContent = null;


        CheckForDuplications();
        SmudgeThePond();
    }

    /// <summary>
    /// Проверяет, что объект не находится по диагонали.
    /// </summary>
    bool NotAngular(Transform pos1)
    {
        return Mathf.Abs(pos1.position.x - transform.position.x) < 0.5f || Mathf.Abs(pos1.position.y - transform.position.y) < 0.5f;
    }


    /// <summary>
    /// Подсвечивает клетку.
    /// </summary>
    public void Highlight()
    {
        cellSprite.color = Color.aliceBlue;
    }

    /// <summary>
    /// Снимает подсветку с клетки.
    /// </summary>
    public void NoHighlight()
    {
        if (cellSprite)
        {
            cellSprite.color = previousColor;
        }
    }


    /// <summary>
    /// Проверяет, является ли текущая клетка соседней с проверяемой.
    /// </summary>
    /// <param name="obj">Клетка для проверки</param>
    /// <returns></returns>
    public bool IsNeighbour(Cell obj)
    {
        foreach (Cell cell in neighbourCells)
        {
            if (cell && cell == obj) return true;
        }
        return false;
    }


    /// <summary>
    /// Проверяет, свободна ли клетка для размещения домино.
    /// </summary>
    public bool IsFreeForDomino()
    {
    return !curDomino;
    }
       
    /// <summary>
    /// Проверяет, полностью ли свободна клетка.
    /// </summary>
    public bool IsFree() { 
        return curDomino == null && curContent == null;
    }
  
    /// <summary>
    /// Проверяет, разрешено ли дублирование объектов в этой клетке.
    /// </summary>
    public bool CheckDuplicationAllowed()
    {
        if (curContent == null && curDomino != null && duplicationBlockers == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Проверяет возможность дублирования и вызывает событие при разрешении.
    /// </summary>
    public void CheckForDuplications()
    {
        if (CheckDuplicationAllowed()) OnDuplicationAllowed?.Invoke(this);
    }

    /// <summary>
    /// Добавляет блокировщик дублирования.
    /// </summary>
    public void AddDuplicationBlocker()
    {
        duplicationBlockers++;
        CheckForDuplications();
    }

    /// <summary>
    /// Удаляет блокировщик дублирования.
    /// </summary>
    public void RemoveDuplicationBlocker()
    {
        duplicationBlockers = Mathf.Max(0, duplicationBlockers - 1);
        CheckForDuplications();
    }

    // ------------------------------------------------------------------------------------------------------------
    void ClearThePond()
    {
        if (Physics2D.OverlapCircle(this.transform.position, .01f, LayerMask.GetMask("Pond")))
        {
            Collider2D pondCol = Physics2D.OverlapCircle(this.transform.position, .01f, LayerMask.GetMask("Pond"));

            Color alpha = pondCol.gameObject.GetComponent<SpriteRenderer>().color;
            alpha.a = 0.5f;
            pondCol.gameObject.GetComponent<SpriteRenderer>().color = alpha;
        }
    }
    void SmudgeThePond()
    {
        if (Physics2D.OverlapCircle(this.transform.position, .01f, LayerMask.GetMask("Pond")))
        {
            Collider2D pondCol = Physics2D.OverlapCircle(this.transform.position, .01f, LayerMask.GetMask("Pond"));

            Color alpha = pondCol.gameObject.GetComponent<SpriteRenderer>().color;
            alpha.a = 1f;
            pondCol.gameObject.GetComponent<SpriteRenderer>().color = alpha;
        }
    }
    // ------------------------------------------------------------------------------------------------------------
}
