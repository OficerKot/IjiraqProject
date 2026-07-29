using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICraftService
{
    /// <summary>
    /// Проверка возможности крафта предмета
    /// </summary>
    /// <param name="item">Предмет для крафта</param>
    public bool CanCraft(ItemData item);

    /// <summary>
    /// Проверка видимости рецепта предмета в окне крафта
    /// </summary>
    public bool IsExplored(ItemData item);

    /// <summary>
    /// Крафт предмета, если имеются необходимые предметы в инвентаре.
    /// </summary>
    /// <param name="item"></param>
    public void Craft(ItemData item);

    public event Action OnRecipesUpdated;
    
}
