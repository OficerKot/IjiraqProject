using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;


public class DominoFactory
{
    SigilsState _sigils;
    DominoConfig _config;
    LayerSorter _layerSorter;
    HandManager _handManager;
    RoadManager _roadManager;

    [Inject]
     void Construct(RoadManager roadManager, HandManager handManager, SigilsState playerSigils, DominoConfig config, LayerSorter layerSorter)
    {
        _roadManager = roadManager;
        _handManager = handManager;
        _layerSorter = layerSorter;
        _sigils = playerSigils;
        _config = config;
    }

    /// <summary>
    /// Генерирует на сцене домино из случайной комбинации доступных сигилов
    /// </summary>
    public Domino GenerateRandomDomino()
    {
        Domino domino = GameObject.Instantiate(_config.dominoPrefab).GetComponent<Domino>();
        DominoPart p1, p2;

        (p1, p2) = SpawnParts(GenerateRandomSigil(), GenerateRandomSigil());
        domino.Construct(_handManager, _layerSorter, p1, p2);

        _layerSorter.Register(domino);
        return domino;
    }
    /// <summary>
    /// Создает игровые объекты для частей домино.
    /// </summary>
    (DominoPart, DominoPart) SpawnParts(SigilInstance sigil1, SigilInstance sigil2)
    {
        DominoPart p1 = GameObject.Instantiate(sigil1.sigilTypeData.prefab).GetComponent<DominoPart>(); 
        DominoPart p2 = GameObject.Instantiate(sigil2.sigilTypeData.prefab).GetComponent<DominoPart>();

        p1.Init(_roadManager, sigil1);
        p2.Init(_roadManager, sigil2);

        return (p1,p2);
    }


    /// <summary>
    /// Получить случайный вариант сигила из всех доступных игроку.
    /// </summary>
    SigilInstance GenerateRandomSigil()
    {
        var (sigilType, boneNumber) = GetRandomSigil();

        SigilInstance sigilInstance = new SigilInstance();
        sigilInstance.Init(sigilType, boneNumber);

        return sigilInstance;
    }
    /// <summary>
    /// Получить случайный сигил с номером из всех доступных игроку.
    /// </summary>
    public (SigilData,int) GetRandomSigil()
    {
        Dictionary<SigilData, HashSet<int>> availableSigils = _sigils.GetAvailableTypes();
        List<SigilData> keys = new List<SigilData>(availableSigils.Keys);

        int indx = Random.Range(0, availableSigils.Count);

        SigilData randSigil = keys[indx];
        int boneNumber = ChooseRandomBoneNumber(availableSigils[randSigil]);

        return (randSigil, boneNumber);
    }

    int ChooseRandomBoneNumber(HashSet<int> availableNumbers)
    {
        int indx = Random.Range(0, availableNumbers.Count);
        int randNumber = availableNumbers.ElementAt(indx);

        return randNumber;
    }

    
}
