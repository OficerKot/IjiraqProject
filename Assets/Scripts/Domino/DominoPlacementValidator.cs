using UnityEngine;

public static class DominoPlacementValidator
{
    public static bool ValidatePlacement(Domino d, Cell c1, Cell c2) {

        if (!c1.HasNeighbourDominos() && !c2.HasNeighbourDominos() && GameManager.Instance.dominoPlaced) return false;
       

        float EPS = 0.2f;

        Cell cellWithMaxCoords = c1;
        Cell cellWithMinCoords = c2;

        if (c2.transform.position.x > c1.transform.position.x + EPS || c2.transform.position.y > c1.transform.position.y + EPS)
        {
            cellWithMaxCoords = c2;
            cellWithMinCoords = c1;
        }


        SigilInstance sigilWithMaxCoords = d.part1.sigilVariantData;
        SigilInstance sigilWithMinCoords = d.part2.sigilVariantData;

        if(d.part2.transform.position.x > d.part1.transform.position.x + EPS || d.part2.transform.position.y > d.part1.transform.position.y + EPS)
        {
            sigilWithMaxCoords = d.part2.sigilVariantData;
            sigilWithMinCoords = d.part1.sigilVariantData;
        }

   
        var rMax = sigilWithMaxCoords.sigilTypeData.placeRequirments;
        var rMin = sigilWithMinCoords.sigilTypeData.placeRequirments;

        if (!rMax.cellRequirements.ValidateCell(cellWithMaxCoords) || !rMin.cellRequirements.ValidateCell(cellWithMinCoords) ) return false;
        if (!CheckNeighbourSigils(cellWithMaxCoords, sigilWithMaxCoords) || !CheckNeighbourSigils(cellWithMinCoords, sigilWithMinCoords)) return false;

        return true;
    }

    static bool CheckNeighbourSigils(Cell c, SigilInstance sigil)
    {
        foreach (var n in c.neighbourCells)
        {
            var neighbourDomino = n.GetCurDomino();
            if (neighbourDomino == null) continue;

            SigilInstance neighbourSigil = neighbourDomino.sigilVariantData;
            PlacementRequirements neighbourRequirements = neighbourSigil.sigilTypeData.placeRequirments.neighboursRequirements;

            if (!neighbourRequirements.ValidateNeighbour(neighbourSigil, sigil)) return false;
            if (!sigil.sigilTypeData.placeRequirments.neighboursRequirements.ValidateNeighbour(sigil, neighbourSigil)) return false;
        }
        return true;
    }

    
}
