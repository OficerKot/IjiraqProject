using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Хранит в себе информацию о каждой части домино и связывает id с их соответствующими префабами.
/// Осуществляет поиск по номеру, изображению или id.
/// </summary>
[CreateAssetMenu(fileName = "DominoManager", menuName = "Domino/DominoManager")]
public class DominoManager : ScriptableObject
{
    public List<SigilData> allSigils;

    public Dictionary<SigilType, int> order = new Dictionary<SigilType, int>()
    {
        {SigilType.Bone, 1} , {SigilType.Fireflies, 2}, {SigilType.Leaves, 3 }, {SigilType.Flowers, 4 }, {SigilType.Axe, 5}, {SigilType.Pickaxe, 6}
    };

    private static DominoManager _instance;

    /// <summary>
    /// Singleton instance менеджера домино. Автоматически загружается из Resources.
    /// </summary>
    public static DominoManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<DominoManager>("DominoManager");

                if (_instance == null)
                {
                    _instance = CreateInstance<DominoManager>();
                    Debug.LogWarning("Created new DominoManager instance. Consider creating it as an asset.");
                }
            }
            return _instance;
        }
    }

    private void OnEnable()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    /// <summary>
    /// Возвращает данные части домино по её идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор домино.</param>
    /// <returns>Данные домино или null, если не найдено.</returns>
    public SigilData GetSigilByID(string id)
    {
        return allSigils.Find(sigil => sigil.characteristics.ID == id);
    }

    /// <summary>
    /// Находит информацию о части домино по изображению.
    /// </summary>
    /// <param name="type">Требуемое изображение.</param>
    /// <returns>Данные домино или null, если не найдено.</returns>
    public SigilData GetSigil(SigilType type)
    {
        return allSigils.Find(d => d.characteristics.sigilType == type);
    }

}

