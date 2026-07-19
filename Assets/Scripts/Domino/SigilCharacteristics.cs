using UnityEngine;

[System.Serializable]
public abstract class SigilCharacteristics
{
    public ImageEnumerator image { get; private set; } = ImageEnumerator.any;
    public ToolType tool { get; private set; } = ToolType.None;
    public int number { get; private set; } = 0;

}
