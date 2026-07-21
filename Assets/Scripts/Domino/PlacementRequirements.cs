using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PlacementRequirements
{
    public bool emptyCell = false;
    public ObjectType objectType = ObjectType.Any;
    public SigilType sigilType = SigilType.Any;
    public int number = 0;

    //При необходимости реализовать в классе-наследнике
    //bool CheckSpecificRequirement()
    //{
    //   ...
    //}

    public bool CheckRequirements(Cell cell)
    {
        if (emptyCell && !cell.IsFree()) return false;

        if (objectType != ObjectType.Any && cell.GetCurContent().GetType() != objectType) return false;

        if ( (number != 0 && number != cell.GetCurDomino().sigilVariantData.boneNumber) &&
            (sigilType != SigilType.Any && cell.GetCurDomino().sigilVariantData.sigilTypeData.characteristics.sigilType != sigilType)) return false;
       

        return true;
    }
}
