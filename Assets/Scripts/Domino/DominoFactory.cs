using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DominoFactory
{
    SigilsState playerSigils;
    GameObject dominoPrefab;
    public void Init(SigilsState playerSigils)
    {
        this.playerSigils = playerSigils;
    }
    public Domino GenerateRandomDomino()
    {
        Domino domino = GameObject.Instantiate(dominoPrefab).GetComponent<Domino>();
        DominoPart p1, p2;

        (p1, p2) = SpawnParts(GenerateRandomSigil(), GenerateRandomSigil());
        domino.SetParts(p1, p2);

        SpawnAndSetPivot(domino);

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
    /// Создает точку вращения (pivot) для домино.
    /// </summary>
    void SpawnAndSetPivot(Domino d)
    {
        Vector2 centerPosition = (d.part1.transform.position + d.part2.transform.position) / 2f;

        d.pivot = new GameObject("Pivot");
        d.pivot.transform.position = centerPosition;
        d.pivot.transform.rotation = d.transform.rotation;

        d.part1.transform.SetParent(d.pivot.transform);
        d.part2.transform.SetParent(d.pivot.transform);
        d.transform.SetParent(d.pivot.transform);
    }
    SigilInstance GenerateRandomSigil()
    {
        SigilInstance sigilInstance = new SigilInstance();

        SigilData sigil = GetRandomSigil();
        int variantIndex = ChooseRandomSigilVariant(sigil);

        sigilInstance.Init(sigil, variantIndex);
        return sigilInstance;
    }
    /// <summary>
    /// Получить случайный сигил из всех доступных игроку.
    /// </summary>
    public SigilData GetRandomSigil()
    {
        List<SigilData> allAvailable = playerSigils.GetAllAvailable();

        int indx = Random.Range(0, allAvailable.Count);
        return allAvailable[indx];
    }

    public int ChooseRandomSigilVariant(SigilData sigil)
    {
        int spritesCnt = sigil.sprites.Length;
        int indx = Random.Range(0, spritesCnt);
        return indx;
    }

    
}
