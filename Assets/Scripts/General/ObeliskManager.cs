using System;
using UnityEngine;

/// <summary>
/// Контроль сбора обелисков.
/// При сборе полного набора обелисков вызывает завершение игры.
/// </summary>
public class ObeliskManager
{
    [SerializeField] int obelisksCount = 4; // количество обелисков на карте с уникальным цветом
    public static event Action<ObeliskColor> OnObeliskCollected;

    /// <summary>
    /// Сбор обелиска и вызов соответствующего события. Проверка на победу.
    /// </summary>
    /// <param name="c"></param>
    public void Pick(ObeliskColor c)
    {
        obelisksCount--;
        OnObeliskCollected.Invoke(c);
        if(obelisksCount == 0)
        {
            GameManager.Instance.Win();
        }
    }
}
