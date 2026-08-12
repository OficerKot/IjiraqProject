using NUnit.Framework;
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
    /// <param name="offsetX">Отступ между клетками по оси Х</param>
    /// <param name="offsetY">Отступ между клетками по оси Y</param>
    /// <param name="startPosition">Позиция левого верхнего угла сетки</param>
    public List<GameObject> PlaceCells(
        int columns,
        int rows,
        float offsetX,
        float offsetY,
        Vector2 startPosition)
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
                    cellWidth,
                    cellHeight,
                    offsetX,
                    offsetY
                );

                spawnedCells.Add(CreateCell(position));
            }
        }

        return spawnedCells;
    }

    private Vector2 CalculatePosition(
        int column,
        int row,
        Vector2 startPosition,
        float cellWidth,
        float cellHeight,
        float offsetX,
        float offsetY)
    {
        return startPosition + new Vector2(
            column * (cellWidth + offsetX),
            row * (cellHeight + offsetY)
        );
    }

    private GameObject CreateCell(Vector2 position)
    {
        GameObject cell = Object.Instantiate(_cellPrefab, _parent);

        RectTransform rectTransform = cell.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;

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
