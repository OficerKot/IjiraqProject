using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

public class SigilsState
{
    [field: SerializeField] Dictionary<SigilData, HashSet<int>> unlocked  = new Dictionary<SigilData, HashSet<int>>();
    [field: SerializeField] List<SigilData> basic = new List<SigilData>();

    public event Action OnUnlockedSigilsChanged;

    DominoManager _dominoManager;
    [Inject]
    public void Construct(DominoManager dominoManager)
    {
        _dominoManager = dominoManager;

        FillBasicSigils();
    }
    
    void FillBasicSigils()
    {
        foreach (var sigil in _dominoManager.allSigils)
        {
            if (sigil.characteristics.isBasic)
            {
                basic.Add(sigil);
            }
            
        }
    }

    /// <summary>
    /// ѕолучить все сигилы, доступные игроку, включа€ базовые
    /// </summary>
    /// <returns>—игилы и соответствующие им доступные номера</returns>
    public Dictionary<SigilData, HashSet<int>> GetAvailableVariants()
    {
        Dictionary<SigilData, HashSet<int>> result = new Dictionary<SigilData, HashSet<int>>();

        foreach (var sigil in basic)
        {
            if(sigil.sprites.Length == 1)
            {
                result.Add(sigil, new HashSet<int>() {0});
            }
            else
            {
                result.Add(sigil, new HashSet<int>() { 1, 2, 3, 4, 5, 6 });
            }
        }
        result.AddRange(unlocked);

        return result;
    }

    /// <summary>
    /// ѕолучить все типы сигилов, доступных игроку, включа€ базовые
    /// </summary>
    /// <returns>“ипы сигилов</returns>
    public List<SigilData> GetAvailableTypes()
    {
        List<SigilData> res = new List<SigilData>(basic);
        res.AddRange(unlocked.Keys);

        return res;
    }

    /// <summary>
    /// ѕровер€ет, есть ли доступные сигилы (кроме базовых).
    /// </summary>
    /// <returns>True если есть доступные сигилы.</returns>
    public bool HasAvailable()
    {
        return unlocked.Count > 0;
    }

    public void UnlockSigil(SigilData data, int num)
    {
        if (!unlocked.TryGetValue(data, out HashSet<int> result))
        {
            unlocked.Add(data, new HashSet<int>());
        }
        
        if(result.Add(num))
        {
            OnUnlockedSigilsChanged?.Invoke();
        }
            
    }

    public void LockSigil(SigilData data, int num)
    {
        if(!unlocked.TryGetValue(data, out HashSet<int> result))
        {
            return;
        }

        if (!result.Remove(num)) return;

        if (result.Count == 0)
        {
            unlocked.Remove(data);
        }

        OnUnlockedSigilsChanged?.Invoke();
    }


}
