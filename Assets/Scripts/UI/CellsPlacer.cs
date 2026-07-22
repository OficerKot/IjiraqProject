using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CellsPlacer : MonoBehaviour
{
    SigilsState sigilsState;

    public GameObject prefab;
    public List<GameObject> spawnedCells = new List<GameObject>();
    HashSet<SigilType> uniqueImages = new HashSet<SigilType>();
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
   

    public void Init(SigilsState state)
    {
        sigilsState = state;
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
        FindUniqueImages();
        elementsCnt = uniqueImages.Count;

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
        foreach (SigilType type in uniqueImages)
        {
            GameObject icon = Instantiate(DominoManager.Instance.GetSigil(type).UIprefab, spawnedCells[indx].transform);
            icon.transform.SetAsFirstSibling();
            spawnedCells[indx].GetComponent<ImageFilterButton>().image = type;
            icon.transform.localPosition = Vector3.zero;
            icon.transform.localScale *= scaleKoef;
            indx++;
        }

    }

    void FindUniqueImages()
    {
        foreach (SigilData d in sigilsState.GetAllAvailable())
        {
            if (!uniqueImages.Contains(d.characteristics.sigilType))
            {
                uniqueImages.Add(d.characteristics.sigilType);
            }
        }
    }
    void ClearElements()
    {
        for (int i = 0; i < spawnedCells.Count; i++)
        {
            Destroy(spawnedCells[i]);
        }
        spawnedCells.Clear();
        uniqueImages.Clear();
    }

}
