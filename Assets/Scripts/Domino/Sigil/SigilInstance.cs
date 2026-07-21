using UnityEngine;

/// <summary>
/// Данные о конкретном созданном сигиле на сцене
/// </summary>
public class SigilInstance : MonoBehaviour
{
    public SigilData sigilTypeData { get; private set; }
    public int variantIndex { get; private set; }
    public int boneNumber => sigilTypeData.sprites.Length > 1 ? variantIndex + 1 : 0; // 0 если инструмент или что то другое, что не может иметь номера
    public Sprite Sprite => sigilTypeData.sprites[variantIndex];

    public void Init(SigilData data, int variantIndex)
    {
        this.sigilTypeData = data;
        this.variantIndex = variantIndex;
    }

}
