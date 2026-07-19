using UnityEngine;

public interface ICellContent
{
    public ObjectType GetType();
    public bool CanBeBrokenBy(Domino d);
    public void Remove();

}
