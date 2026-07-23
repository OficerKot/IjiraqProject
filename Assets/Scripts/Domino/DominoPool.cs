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
        List<Domino> dominoList = new List<Domino>(number);
        for (int i = 0; i < number; i++)
        {
            Domino newDomino = factory.GenerateRandomDomino();
            dominoList.Add(newDomino);
        }
        currentPool = dominoList;
    }

}
