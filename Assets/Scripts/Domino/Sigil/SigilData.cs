using UnityEngine;
/// <summary>
/// Хранит всю информацию о кокретном типе сигила
/// </summary>
/// 
[CreateAssetMenu(fileName = "New domino type", menuName = "Domino/DominoData")]
public class SigilData : ScriptableObject
{
    public SigilCharacteristics characteristics;
    public DominoRequirements placeRequirments;

    public Sprite[] sprites;
    [field: SerializeField] public GameObject prefab { get; private set; }
}
