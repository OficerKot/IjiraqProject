using UnityEngine;

public static class DominoPlacementValidator
{
    static bool CheckNeighbourSigils(Cell c, SigilInstance sigil) {
        foreach (var n in c.neighbourCells)
        {
            var neighbourDomino = n.GetCurDomino();
            if (neighbourDomino == null) continue;

            SigilInstance neighbourSigil = neighbourDomino.sigilVariantData;
            PlacementRequirements neighbourRequirements = neighbourSigil.sigilTypeData.placeRequirments.neighboursRequirements;

            if (!neighbourRequirements.ValidateNeighbour(neighbourSigil, sigil)) return false;
            if (!sigil.sigilTypeData.placeRequirments.neighboursRequirements.ValidateNeighbour(sigil, neighbourSigil)) return false;
        }
        return true; }

    public static bool ValidatePlacement(Domino d, Cell c1, Cell c2) {

        float dist1_1 = Vector2.Distance(d.part1.transform.position, c1.transform.position);
        float dist2_1 = Vector2.Distance(d.part2.transform.position, c1.transform.position);

        SigilInstance sigilForC1 = d.part1.sigilVariantData;
        SigilInstance sigilForC2 = d.part2.sigilVariantData;

        if (dist1_1 > dist2_1)
        {
            sigilForC1 = d.part2.sigilVariantData;
            sigilForC2 = d.part1.sigilVariantData;
        }

        var r1 = sigilForC1.sigilTypeData.placeRequirments;
        var r2 = sigilForC2.sigilTypeData.placeRequirments;

        if (!r1.cellRequirements.ValidateCell(c1) || !r2.cellRequirements.ValidateCell(c2) ) return false;
        if (!CheckNeighbourSigils(c1, sigilForC1) || !CheckNeighbourSigils(c2, sigilForC2)) return false;

        return true;
    }
}
