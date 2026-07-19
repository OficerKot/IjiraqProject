using UnityEngine;
/// <summary>
/// Информация о части домино, хранит в себе
/// название, номер рисунка и номер рисунка соседних домино, изображение и изображение соседних домино,
/// игровой и ui префабы
/// </summary>
/// 
[CreateAssetMenu(fileName = "New sigil type", menuName = "Sigil/SigilData")]
public class SigilData : ScriptableObject
{
    public string sigilId;

    public SigilCharacteristics characteristics;
    public DominoRequirements placeRequirments;

    public GameObject prefab;
    public GameObject UIprefab;
}
