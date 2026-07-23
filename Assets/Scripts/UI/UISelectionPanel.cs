using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UISelectionPanel : PauseBehaviour
{
    [SerializeField] List<GameObject> spawnedUIDomino = new List<GameObject>();
    DominoPool dominoPool;
    bool isActive = true;

    [Header("Настройки внешнего вида домино")]
    [SerializeField] GameObject UIDominoPrefab;
    [SerializeField] GameObject UISigilPrefab;

    [Header("Настройки расположения домино")]
    public float YPos = -170;
    public float XPos = 5;
    public float XOffset = 50;

    public void Init(DominoPool dominoPool)
    {
        this.dominoPool = dominoPool;
    }


    public override void OnGamePaused(bool isGamePaused)
    {
        isActive = !isGamePaused;
    }
    public void UpdatePanel()
    {
        if (!isActive) return;

        UpdateHunger();
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
            Debug.Log(spawnedUIDomino.Count);
            foreach (var c in spawnedUIDomino)
            {
                Debug.Log(c.name);
                Hunger.Instance.MakeStep();
                Destroy(c);
            }
            spawnedUIDomino.Clear();
        }
    }

    public void DisplayDomino(Domino domino, Vector3 pos)
    {
        GameObject uiDomino = Instantiate(UIDominoPrefab, transform);

        RectTransform uiDominoRect = uiDomino.GetComponent<RectTransform>();
        uiDominoRect.localPosition = pos;

        Image part1Sprite = Instantiate(UISigilPrefab, uiDomino.transform).GetComponent<Image>();
        Image part2Sprite = Instantiate(UISigilPrefab, uiDomino.transform).GetComponent<Image>();

        part1Sprite.sprite = domino.part1.sigilVariantData.Sprite;
        part2Sprite.sprite = domino.part2.sigilVariantData.Sprite;

        spawnedUIDomino.Add(uiDomino);
    }

    public void RemoveDomino(GameObject d)
    {
        spawnedUIDomino.Remove(d);
        Destroy(d);
    }

}
