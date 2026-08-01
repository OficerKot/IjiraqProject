using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Rendering;
using VContainer.Unity;

public interface ILayerSortable
{
    SortingGroup sortingGroup { get; }
    SortingOrder defaultSortingOrder { get; }

    event Action<ILayerSortable> Picked;
    event Action<ILayerSortable> Placed;
}
