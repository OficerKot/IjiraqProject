using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Порядок сортировки слоёв по умолчанию.
/// </summary>
public enum SortingOrder
{
    domino = 3,
    item = 4,
    pond = 5,
    player = 6,
    web = 10
}

/// <summary>
/// Предназначен для смены порядка слоёв в процессе игры.
/// </summary>
public class LayerSorter
{
    int topLayer = 11;
    /// <summary>
    /// Содержит объекты, перемещённые на передний план в порядке их добавления.
    /// </summary>
    List<ILayerSortable> objectsOnTop = new List<ILayerSortable>();

    public void Register(ILayerSortable sortable)
    {
        sortable.Picked += PutInFront;
        sortable.Placed += PutBack;
    }

    public void Unregister(ILayerSortable sortable)
    {
        sortable.Picked -= PutInFront;
        sortable.Placed -= PutBack;
    }
    public void PutInFront(ILayerSortable obj)
    {
        SortingGroup sortingGroup = obj.sortingGroup;
        objectsOnTop.Add(obj);
        sortingGroup.sortingOrder = topLayer;
        topLayer++;
    }

    public void PutBack(ILayerSortable obj)
    {
        if (objectsOnTop.Contains(obj))
        {
            obj.sortingGroup.sortingOrder = (int)obj.defaultSortingOrder;
            if (objectsOnTop[objectsOnTop.Count-1] == obj)
            {
                topLayer--;
            }
            objectsOnTop.Remove(obj);
        }
    }
}
