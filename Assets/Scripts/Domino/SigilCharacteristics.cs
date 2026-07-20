using UnityEngine;

[System.Serializable]
public class SigilCharacteristics
{
    public string ID;
    [field: SerializeField]  public int number { get; private set; } = 0;
    [field: SerializeField]  public SigilType sigilType { get; private set; } = SigilType.Any;
    [field: SerializeField]  public ToolType tool { get; private set; } = ToolType.None;
   

}
