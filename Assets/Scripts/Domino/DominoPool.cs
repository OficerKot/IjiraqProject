using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using VContainer;

public class DominoPool
{
    public List<Domino> currentPool { get; private set; } = new List<Domino>();
    DominoFactory _factory;

    [Inject]
    public void Construct(DominoFactory factory)
    {
        _factory = factory;
    }
    public void UpdateDominoSet(int number)
    {
        DestroyUnspawned();

        List<Domino> dominoList = new List<Domino>(number);
        for (int i = 0; i < number; i++)
        {
            Domino newDomino = _factory.GenerateRandomDomino();
            newDomino.OnPlaced += OnDominoPlaced;

            newDomino.pivot.SetActive(false);
            dominoList.Add(newDomino);
        }
        currentPool = dominoList;
    }

    void OnDominoPlaced(Domino d)
    {
        d.OnPlaced -= OnDominoPlaced;
        currentPool.Remove(d);
    }

    void DestroyUnspawned()
    {
        foreach (var domino in currentPool)
        {
            domino.OnPlaced -= OnDominoPlaced;
            GameObject.Destroy(domino.pivot);
        }
        currentPool.Clear();
    }

}
