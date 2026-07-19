using UnityEngine;

public static class DominoPlacementValidator
{
    static bool CheckCell(Cell c, PlacementRequirements r) {
        return r.CheckRequirements(c);
    }
    static bool CheckCellNeighbors(Cell c, PlacementRequirements r) {

        foreach (var n in c.neighbourCells)
        {
            if (!CheckCell(n, r)) return false;
        }
        return true; }

    public static  bool ValidatePlacement(Domino d) {
     
        DominoRequirements r1 = d.part1.data.placeRequirments;
        DominoRequirements r2 = d.part2.data.placeRequirments;

        Cell c1 = d.curCell1;
        Cell c2 = d.curCell2;

        if (!CheckCell(c1, r1.cellRequirements) || !CheckCell(c2, r2.cellRequirements)) return false;

        if (!CheckCellNeighbors(c1, r1.neighboursRequirements) || !CheckCellNeighbors(c2, r2.neighboursRequirements)) return false;

        return true;
    }
}
