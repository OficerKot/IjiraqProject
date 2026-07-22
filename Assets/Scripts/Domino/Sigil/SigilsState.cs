using System.Collections.Generic;
using UnityEngine;

public class SigilsState : MonoBehaviour
{
    DominoManager dominoManager;
    public List<SigilData> available { get; private set; } = new List<SigilData>();
    public List<SigilData> basic { get; private set; } = new List<SigilData>();

    public void Init(DominoManager dominoManager)
    {
        this.dominoManager = dominoManager;
        FillBasicSigils();
    }
    
    void FillBasicSigils()
    {
        foreach (var sigil in dominoManager.allSigils)
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
    /// <returns>Cписок сигилов SigilData</returns>
    public List<SigilData> GetAllAvailable()
    {
        List<SigilData> result = new List<SigilData>(basic);
        result.AddRange(available);
        return result;
    }

    /// <summary>
    /// ѕровер€ет, есть ли доступные сигилы (кроме базовых).
    /// </summary>
    /// <returns>True если есть доступные сигилы.</returns>
    public bool HasAvailable()
    {
        return available.Count > 0;
    }

    public void AddToAvailable(SigilData data)
    {
        throw new System.NotImplementedException();
    }

    public void RemoveFromAvailable(SigilData data)
    {
        throw new System.NotImplementedException();
    }


}
