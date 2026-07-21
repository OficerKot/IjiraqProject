using System.Collections.Generic;
using UnityEngine;

public class SigilsState : MonoBehaviour
{
    DominoManager dominoManager;
    public List<SigilData> available { get; private set; }
    public List<SigilData> basic { get; private set; }

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
    /// Получить все сигилы, доступные игроку, включая базовые
    /// </summary>
    /// <returns>Cписок сигилов SigilData</returns>
    public List<SigilData> GetAllAvailable()
    {
        List<SigilData> result = new List<SigilData>(basic);
        result.AddRange(available);
        return result;
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
