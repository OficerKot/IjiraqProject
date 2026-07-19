using UnityEngine;

public interface ICellContent
{
    public ObjectType GetType();
    public bool CanBeBrokenBy(DominoPart cur, DominoPart other);
    public void Break();

}
