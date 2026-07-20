using UnityEngine;
/// <summary>
/// Информация о части домино, хранит в себе
/// название, номер рисунка и номер рисунка соседних домино, изображение и изображение соседних домино,
/// игровой и ui префабы
/// </summary>
/// 
[CreateAssetMenu(fileName = "New domino type", menuName = "Domino/DominoData")]
public class DominoData : ScriptableObject
{
    public SigilCharacteristics characteristics;
    public DominoRequirements placeRequirments;

    public Sprite[] Sprites;
    [field: SerializeField] public GameObject prefab { get; private set; }
    [SerializeField] public GameObject UIprefab; // это убрать!
}
