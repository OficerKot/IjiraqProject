using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Используется для создания сетки из клеток внутри родителя
/// </summary>
public class UICellsPlacer
{
    private readonly GameObject _cellPrefab;
    private readonly Transform _parent;
    List<GameObject> spawnedCells = new List<GameObject>();

    public UICellsPlacer(GameObject cellPrefab, Transform parent)
    {
        _cellPrefab = cellPrefab;
        _parent = parent;
    }

    /// <summary>
    /// Создаёт сетку из префабов внутри родителя
    /// </summary>
    /// <param name="columns">Количество клеток в ширину</param>
    /// <param name="rows">Количество клеток в высоту</param>
    /// <param name="startPosition">Позиция левого верхнего угла сетки</param>
    /// <param name="spacing">Отступы между клетками по осям x и y</param>
    public List<GameObject> CreateGrid(
        int columns,
        int rows,
        Vector2 startPosition,
        Vector2 spacing)
    {
        float cellWidth = GetCellWidth();
        float cellHeight = GetCellHeight();

        for (int row = rows; row > 0; row--)
        {
            for (int column = 0; column < columns; column++)
            {
                Vector2 position = CalculatePosition(
                    column,
                    row,
                    startPosition,
                    spacing,
                    cellWidth,
                    cellHeight                  
                );

                spawnedCells.Add(CreateCell(position));
            }
        }

        return spawnedCells;
    }

    public List<GameObject> CreateRow(
      int rowWidth,
      float spacing,
      Vector2 startPosition)
    {
        float cellWidth = GetCellWidth();
        float cellHeight = GetCellHeight();

        for (int column = 0; column < rowWidth; column++)
        {
                Vector2 position = CalculatePosition(
                    column,
                    startPosition,
                    spacing,
                    cellWidth,
                    cellHeight
                );
                spawnedCells.Add(CreateCell(position));
        }

        return spawnedCells;
    }

    private Vector2 CalculatePosition(
        int column,
        int row,
        Vector2 startPosition,
        Vector2 spacing,
        float cellWidth,
        float cellHeight
        )
    {
        return startPosition + new Vector2(
            column * (cellWidth + spacing.x),
            row * (cellHeight + spacing.y)
        );
    }

    private Vector2 CalculatePosition(
        int column,
        Vector2 startPosition,
        float spacing,
        float cellWidth,
        float cellHeight
        )
    {
        return startPosition + new Vector2(column * (cellWidth + spacing), startPosition.y);
    }

    private GameObject CreateCell(Vector2 position)
    {
        GameObject cell = Object.Instantiate(_cellPrefab, _parent);

        RectTransform rectTransform = cell.GetComponent<RectTransform>();
        rectTransform.localPosition = position;

        return cell;
    }

    private float GetCellWidth()
    {
        return _cellPrefab.GetComponent<RectTransform>().rect.width;
    }

    private float GetCellHeight()
    {
        return _cellPrefab.GetComponent<RectTransform>().rect.height;
    }
}
