using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VContainer; 

public class SigilsMenuIconsPlacer : MonoBehaviour
{
    SigilsState _sigilsState;
    DominoConfig _config;

    public GameObject prefab;
    public List<GameObject> spawnedCells = new List<GameObject>();
    HashSet<Sprite> icons = new HashSet<Sprite>();
    public BoxCollider2D windowCollider;

    public Vector3 startPos;
    Vector3 curStartPos;

    [SerializeField] float defaultOffset;
    [SerializeField] float scaleKoef = 3f;
    float offsetX;
    float midX;

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
        PlaceElements();
        FillIcons();
    }
    private void Start()
    {
        PlaceElements();
        FillIcons();
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

    void PlaceElements()
    {
        FillImages();
        elementsCnt = icons.Count;

        if (elementsCnt < maxDefaultElements)
        {
            float cellWidth = prefab.GetComponent<BoxCollider2D>().size.x * prefab.transform.lossyScale.x;
            midX = windowCollider.transform.localPosition.x;
            curStartPos = new Vector3(midX - cellWidth * (elementsCnt - 1) / 2, startPos.y, startPos.z);

        }
        else
        {
            curStartPos = startPos;
        }

        CountOffset();
        for (int i = 0; i < elementsCnt; i++)
        {
            GameObject newObj = Instantiate(prefab, transform);
            newObj.transform.localPosition = curStartPos + new Vector3(offsetX * i, 0, 0);
            spawnedCells.Add(newObj);
        }

    }

    void FillIcons()
    {
        int indx = 0;
        foreach (var i in icons)
        {
            GameObject newIcon = Instantiate(_config.UISigilPrefab, spawnedCells[indx].transform);
            newIcon.GetComponent<Image>().sprite = i;

            newIcon.transform.SetAsFirstSibling();
            spawnedCells[indx].GetComponent<ImageFilterButton>().image = i;
            newIcon.transform.localPosition = Vector3.zero;
            newIcon.transform.localScale *= scaleKoef;
            indx++;
        }

    }

    void FillImages()
    {
        foreach (var sigil in _sigilsState.GetAllAvailable())
        {
            icons.Add(sigil.sprites[0]);
        }
    }
    void ClearElements()
    {
        for (int i = 0; i < spawnedCells.Count; i++)
        {
            Destroy(spawnedCells[i]);
        }
        spawnedCells.Clear();
        icons.Clear();
    }

}
