using UnityEngine;
/// <summary>
/// Хранит данные о доступном для установки домино (для клетки)
/// </summary>
/// 
[System.Serializable]
public class DominoRequirements
{
    [Header("Требования для клетки")]
    public PlacementRequirements cellRequirements;

    [Header("Требования для соседних домино")]
    public PlacementRequirements neighboursRequirements;

}


