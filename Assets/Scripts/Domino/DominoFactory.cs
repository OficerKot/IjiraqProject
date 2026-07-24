using System.Collections.Generic;
using UnityEngine;


public class DominoFactory
{
    SigilsState playerSigils;
    DominoConfig config;
    public void Init(SigilsState playerSigils, DominoConfig config)
    {
        this.playerSigils = playerSigils;
        this.config = config;
    }

    /// <summary>
    /// Генерирует на сцене домино из случайной комбинации доступных сигилов
    /// </summary>
    public Domino GenerateRandomDomino()
    {
        Domino domino = GameObject.Instantiate(config.dominoPrefab).GetComponent<Domino>();
        DominoPart p1, p2;

        (p1, p2) = SpawnParts(GenerateRandomSigil(), GenerateRandomSigil());
        domino.Init(p1, p2);

        return domino;
    }
    /// <summary>
    /// Создает игровые объекты для частей домино.
    /// </summary>
    (DominoPart, DominoPart) SpawnParts(SigilInstance sigil1, SigilInstance sigil2)
    {
        DominoPart p1 = GameObject.Instantiate(sigil1.sigilTypeData.prefab).GetComponent<DominoPart>(); 
        DominoPart p2 = GameObject.Instantiate(sigil2.sigilTypeData.prefab).GetComponent<DominoPart>();

        p1.Init(sigil1);
        p2.Init(sigil2);

        return (p1,p2);
       // CheckPartRotation();
    }


    /// <summary>
    /// Получить случайный вариант сигила из всех доступных игроку.
    /// </summary>
    SigilInstance GenerateRandomSigil()
    {
        SigilInstance sigilInstance = new SigilInstance();

        SigilData sigil = GetRandomSigil();
        int variantIndex = ChooseRandomSigilVariant(sigil);

        sigilInstance.Init(sigil, variantIndex);
        return sigilInstance;
    }
    /// <summary>
    /// Получить случайный тип сигила из всех доступных игроку.
    /// </summary>
    public SigilData GetRandomSigil()
    {
        List<SigilData> allAvailable = playerSigils.GetAllAvailable();

        int indx = Random.Range(0, allAvailable.Count);
        return allAvailable[indx];
    }

    int ChooseRandomSigilVariant(SigilData sigil)
    {
        int spritesCnt = sigil.sprites.Length;
        int indx = Random.Range(0, spritesCnt);
        return indx;
    }

    
}
