using UnityEngine;

[System.Serializable]
public class SigilCharacteristics
{
    [field: SerializeField] public string ID { get; private set; }
    [field: SerializeField] public bool isBasic { get; private set; } = false;
    [field: SerializeField]  public SigilType sigilType { get; private set; } = SigilType.Any;
    [field: SerializeField]  public ToolType tool { get; private set; } = ToolType.None;
   

}
