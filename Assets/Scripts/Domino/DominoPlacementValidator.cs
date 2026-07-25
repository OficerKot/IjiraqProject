using UnityEngine;

public static class DominoPlacementValidator
{
    static bool CheckCell(Cell c, PlacementRequirements r) {
        return r.CheckRequirements(c);
    }
    static bool CheckCellNeighbors(Cell c, PlacementRequirements r) {
        Debug.Log("Cell: " + c);
        foreach (var n in c.neighbourCells)
        {
            if (!CheckCell(n, r)) return false;
        }
        return true; }

    public static bool ValidatePlacement(Domino d, Cell c1, Cell c2) {

        SigilInstance sigil1Data = d.part1.sigilVariantData;
        SigilInstance sigil2Data = d.part2.sigilVariantData;

        DominoRequirements r1 = sigil1Data.sigilTypeData.placeRequirments;
        DominoRequirements r2 = sigil2Data.sigilTypeData.placeRequirments;

        if (!CheckCell(c1, r1.cellRequirements) || !CheckCell(c2, r2.cellRequirements)) return false;

        if (!CheckCellNeighbors(c1, r1.neighboursRequirements) || !CheckCellNeighbors(c2, r2.neighboursRequirements)) return false;

        return true;
    }
}
