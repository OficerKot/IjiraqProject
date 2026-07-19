using UnityEngine;
/// <summary>
/// Хранит данные о доступном для установки домино (для клетки)
/// </summary>
/// 
[System.Serializable]
public class DominoRequirements
{
    public PlacementRequirements cellRequirements;
    public PlacementRequirements neighboursRequirements;

}


