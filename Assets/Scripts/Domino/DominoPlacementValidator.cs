using UnityEngine;

public class DominoPlacementValidator
{
    bool CheckCell(Cell c, DominoPart p) {
        PlacementRequirements requirements = p.data.placeRequirments.cellRequirements;

        return true;
    }
    bool CheckCellNeighbors(Cell c, DominoPart p) {  return true; }
    public bool ValidatePlacement(Domino d) {
        DominoPart p1 = d.part1;
        DominoPart p2 = d.part2;

        Cell c1 = d.curCell1;
        Cell c2 = d.curCell2;

        if (!CheckCell(c1, p1) || !CheckCell(c2, p2)) return false;
        if (!CheckCellNeighbors(c1, p1) || !CheckCellNeighbors(c2, p2)) return false;

        return true;
    }
}
