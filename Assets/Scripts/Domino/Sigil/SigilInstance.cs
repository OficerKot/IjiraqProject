using UnityEngine;

/// <summary>
/// Данные о конкретном созданном сигиле на сцене
/// </summary>
public class SigilInstance
{
    public SigilData sigilTypeData { get; private set; }
    public int boneNumber { get; private set; }
    public Sprite Sprite => boneNumber == 0? sigilTypeData.sprites[0] : sigilTypeData.sprites[boneNumber-1];

    public void Init(SigilData data, int boneNumber)
    {
        this.sigilTypeData = data;
        this.boneNumber = sigilTypeData.sprites.Length > 1 ? boneNumber : 0;
    }

}
