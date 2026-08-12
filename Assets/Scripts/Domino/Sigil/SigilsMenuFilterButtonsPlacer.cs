using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VContainer; 

public class SigilsMenuFilterButtonsPlacer : MonoBehaviour
{
    SigilsState _sigilsState;
    DominoConfig _config;

    public GameObject buttonPrefab;
    public List<GameObject> spawnedCells = new List<GameObject>();
    HashSet<SigilData> sigils = new HashSet<SigilData>();

    [SerializeField] float yPos;
    Vector2 startPos;

    [SerializeField] float defaultOffset;
    [SerializeField] float scaleKoef = 3f;
    float offsetX;

    [SerializeField] int maxDefaultElements = 9;
    int elementsCnt;

    //Чем больше количество элементов, тем меньше отступ
    //При этом если элементов <= maxDefaultElements, отступ будет не больше defaultOffset


    [Inject]
    public void Init(SigilsState state, DominoConfig config)
    {
        _sigilsState = state;
        _config = config;
    }
    public void UpdateButtons()
    {
        ClearElements();

        FillImages();
        elementsCnt = sigils.Count;

        CountOffset();
        CountStartPosXForCenter();

        PlaceElements();
        FillIcons();
    }

    void ClearElements()
    {
        for (int i = 0; i < spawnedCells.Count; i++)
        {
            Destroy(spawnedCells[i].gameObject);
        }
        spawnedCells.Clear();
        sigils.Clear();
    }
    void CountOffset()
    {

        if (elementsCnt <= maxDefaultElements)
        {
            offsetX = defaultOffset;
        }
        else
        {
            float k = (float)maxDefaultElements / elementsCnt;
            offsetX = defaultOffset * k;
        }
    }

    void CountStartPosXForCenter()
    {
        float cellWidth = buttonPrefab.GetComponent<RectTransform>().rect.width;

        float rowWidth =
            elementsCnt * cellWidth +
            (elementsCnt - 1) * offsetX;

        float startX =
            -rowWidth / 2f +
            cellWidth / 2f;

        startPos = new Vector2(startX, yPos);
    }
    void PlaceElements()
    {
        float cellWidth = buttonPrefab.GetComponent<RectTransform>().rect.width;

        for (int i = 0; i < elementsCnt; i++)
        {
            GameObject newObj = Instantiate(buttonPrefab, transform);

            RectTransform rect = newObj.GetComponent<RectTransform>();

            rect.anchoredPosition =
                startPos + new Vector2((cellWidth + offsetX) * i, 0);

            spawnedCells.Add(newObj);
        }
    }

    void FillIcons()
    {
        int indx = 0;
        foreach (var i in sigils)
        {
            GameObject newIcon = Instantiate(_config.UISigilPrefab, spawnedCells[indx].transform);
            newIcon.GetComponent<Image>().sprite = i.sprites[0];

            newIcon.transform.SetAsFirstSibling();
            spawnedCells[indx].GetComponent<ImageFilterButton>().sigil = i;
            newIcon.transform.localPosition = Vector3.zero;
            newIcon.transform.localScale *= scaleKoef;
            indx++;
        }

    }

    void FillImages()
    {
        foreach (var sigil in _sigilsState.GetAvailableTypes())
        {
            SigilData data = sigil.Key;
            sigils.Add(data);
        }
    }

}
