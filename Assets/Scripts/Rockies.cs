using UnityEngine;

/// <summary>
/// Особый тип ресурса "Камешки", с генерацией случайного количества и проверкой по числу на домино.
/// </summary>
public class Rockies : ResourceSource
{
    RockiesNumberGenerator generator;

    private void Awake()
    {
        generator = GetComponent<RockiesNumberGenerator>();
        generator.Generate();
    }

    /// <summary>
    /// Подбирает камешки с учетом сгенерированного количества.
    /// </summary>
    public override void Break()
    {
        Inventory.Instance.AddItem(resource, generator.GetCount());
        curCell.SetCurContent(null);
        Destroy(gameObject);
    }

    /// <summary>
    /// Размещает камешки в клетке и устанавливает для неё сгенерированное число.
    /// </summary>
    /// <param name="cell">Клетка для размещения.</param>
    public override void PutInCell(Cell cell)
    {
        base.PutInCell(cell);
    }

    /// <summary>
    /// Проверяет, можно ли сломать камешки с помощью указанной части домино.
    /// </summary>
    /// <param name="cur">Часть домино для проверки.</param>
    /// <param name="other">Вторая часть домино (пока не используется).</param>
    /// <returns>True если число на домино совпадает с сгенерированным числом камней.</returns>
    public override bool CanBeBrokenBy(DominoPart cur, DominoPart other)
    {
        return cur.data.characteristics.number == generator.GetCount();
    }
}