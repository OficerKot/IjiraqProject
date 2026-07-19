using Unity.VisualScripting;
using UnityEngine;


public class PlacementRequirements
{
    public bool emptyCell = false;
    public ObjectType objectType = ObjectType.Any;
    public ImageEnumerator image = ImageEnumerator.any;
    public int number = 0;

    //При необходимости реализовать в классе-наследнике
    //bool CheckSpecificRequirement()
    //{
    //   ...
    //}

    public bool CheckRequirements(Cell cell)
    {
        if (emptyCell && !cell.IsFree()) return false;

        if (objectType != ObjectType.Any && cell.GetCurContent(). != objectType) return false;

        if ( (number != 0 && number != cell.GetCurDomino().data.characteristics.number) &&
            (image != ImageEnumerator.any && cell.GetCurDomino().data.characteristics.image != image)) return false;
       

        return true;
    }
}
