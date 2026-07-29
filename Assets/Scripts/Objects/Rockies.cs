using UnityEngine;
using VContainer;

/// <summary>
/// Особый тип ресурса "Камешки", с генерацией случайного количества и проверкой по числу на домино.
/// </summary>
public class Rockies : ResourceSource
{
    RockiesNumberGenerator generator;
    private IInventory _inventory;

    [Inject]
    private void Construct(IInventory inventory)
    {
        _inventory = inventory;
    }

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
        _inventory.AddItems(resource, generator.GetCount());
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

        return (c1 == curCell && p1.sigilVariantData.boneNumber == generator.GetCount() ||
            c2 == curCell && p2.sigilVariantData.boneNumber == generator.GetCount());

    }
}