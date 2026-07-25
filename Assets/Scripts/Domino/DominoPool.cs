using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class DominoPool : MonoBehaviour
{
    public List<Domino> currentPool;
    DominoFactory factory;

    public void Init(DominoFactory factory)
    {
        this.factory = factory;
    }
    public void UpdateDominoSet(int number)
    {
        DestroyUnspawned();

        List<Domino> dominoList = new List<Domino>(number);
        for (int i = 0; i < number; i++)
        {
            Domino newDomino = factory.GenerateRandomDomino();
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
            Destroy(domino.pivot);
        }
        currentPool.Clear();
    }

}
