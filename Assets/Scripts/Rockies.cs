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
    public override void Remove()
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
    /// <returns>True если число на домино совпадает с сгенерированным числом камней.</returns>
    public override bool CanBeBrokenBy(Domino d)
    {
        Cell c1 = d.curCell1;
        Cell c2 = d.curCell2;
        DominoPart p1 = d.part1;
        DominoPart p2 = d.part2;

        return (c1 == curCell && p1.data.characteristics.number == generator.GetCount() ||
            c2 == curCell && p2.data.characteristics.number == generator.GetCount());

    }
}