using UnityEngine;

[CreateAssetMenu(fileName = "DominoConfig", menuName = "Domino/DominoConfig")]
public class DominoConfig : ScriptableObject
{
    [Header("Настройки внешнего вида домино")]
    [SerializeField] public GameObject dominoPrefab;
    [SerializeField] public GameObject UIDominoPrefab;
    [SerializeField] public GameObject UISigilPrefab;

}
