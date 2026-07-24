using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UISelectionPanel : PauseBehaviour
{
    [SerializeField] List<UIDomino> spawnedUIDomino = new List<UIDomino>();
    DominoConfig config;
    DominoPool dominoPool;
    bool isActive = true;

    [Header("Настройки расположения домино на панели")]
    [SerializeField] public float YPos = -170;
    [SerializeField] public float XPos = 5;
    [SerializeField] public float XOffset = 50;

    public void Init(DominoPool dominoPool, DominoConfig config)
    {
        this.dominoPool = dominoPool;
        this.config = config;
    }


    public override void OnGamePaused(bool isGamePaused)
    {
        isActive = !isGamePaused;
    }
    public void UpdatePanel()
    {
        if (!isActive) return;

        UpdateHunger();
        ClearPanel();

        int offset = 0;
        foreach (var domino in dominoPool.currentPool)
        {
            Vector3 pos = new Vector3(XPos + offset * XOffset, YPos, 0);
            DisplayDomino(domino, pos);

            offset++;
        }
    }

    void UpdateHunger()
    {
        if (spawnedUIDomino.Count != 0)
        {
            foreach (var c in spawnedUIDomino)
            {
                Hunger.Instance.MakeStep();
            }
        }
    }

    void ClearPanel()
    {
        foreach (var c in spawnedUIDomino)
        {
            Destroy(c);
        }
        spawnedUIDomino.Clear();
    }
    public void DisplayDomino(Domino domino, Vector3 pos)
    {
        UIDomino uiDomino = Instantiate(config.UIDominoPrefab, transform).GetComponent<UIDomino>();
        uiDomino.Init(domino, config);

        RectTransform uiDominoRect = uiDomino.GetComponent<RectTransform>();
        uiDominoRect.localPosition = pos;


        spawnedUIDomino.Add(uiDomino);
    }

}
