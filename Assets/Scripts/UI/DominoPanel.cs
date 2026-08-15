using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;


public class DominoPanel : PauseBehaviour
{
    [SerializeField] List<UIDomino> spawnedUIDomino = new List<UIDomino>();
    UIConfig _config;
    DominoPool _dominoPool;
    HandManager _handManager;
    bool isActive = true;

    [Header("Настройки расположения домино на панели")]
    [SerializeField] public float YPos = -170;
    [SerializeField] public float XPos = 5;
    [SerializeField] public float XOffset = 50;

    [Inject]
    public void Construct(DominoPool dominoPool, UIConfig config, HandManager handManager)
    {
        _dominoPool = dominoPool;
        _config = config;
        _handManager = handManager;
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
        foreach (var domino in _dominoPool.currentPool)
        {
            Vector3 pos = new Vector3(XPos + offset * XOffset, YPos, 0);
            DisplayDomino(domino, pos);

            offset++;
        }
    }

    void UpdateHunger() // убрать
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
            c.OnDestroyed -= RemoveDomino;
            Destroy(c.gameObject);
        }
        spawnedUIDomino.Clear();
    }

    void RemoveDomino(UIDomino domino)
    {
        domino.OnDestroyed -= RemoveDomino;
        spawnedUIDomino.Remove(domino);
    }
    public void DisplayDomino(Domino domino, Vector3 pos)
    {
        UIDomino uiDomino = Instantiate(_config.dominoBase, transform).GetComponent<UIDomino>();
        uiDomino.Init(_handManager, domino, _config);

        uiDomino.OnDestroyed += RemoveDomino;

        RectTransform uiDominoRect = uiDomino.GetComponent<RectTransform>();
        uiDominoRect.localPosition = pos;


        spawnedUIDomino.Add(uiDomino);
    }

}
