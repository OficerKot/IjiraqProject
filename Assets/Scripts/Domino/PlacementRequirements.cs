using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PlacementRequirements
{
    public bool emptyCell = false;
    public ObjectType objectType = ObjectType.Any;
    public SigilType sigilType = SigilType.Any;

    //При необходимости реализовать в классе-наследнике
    //bool CheckSpecificRequirement()
    //{
    //   ...
    //}
    
    public bool ValidateCell(Cell cell)
    {
        if (emptyCell && !cell.IsFree()) return false;

        if (objectType != ObjectType.Any && cell.GetCurContent().GetType() != objectType) return false;

        return true;
    }

    public bool ValidateNeighbour(SigilInstance thisSigil, SigilInstance neighbourSigil)
    {
        if (!CheckNumber(thisSigil.boneNumber, neighbourSigil.boneNumber) && !CheckType(neighbourSigil)) return false;

        return true;
    }

    bool CheckNumber(int thisSigil, int neighbourSigil)
    {
        if (thisSigil * neighbourSigil != 0 && thisSigil != neighbourSigil) return false;
        return true;
    }

    bool CheckType(SigilInstance neighbourSigil)
    {
        if (sigilType != SigilType.Any && sigilType != neighbourSigil.sigilTypeData.characteristics.sigilType ) return false;
        return true;
    }
}
