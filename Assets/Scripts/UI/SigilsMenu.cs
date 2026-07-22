using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SigilsMenu : MonoBehaviour, IMenu
{
    SigilsState sigilsState;
    CellsPlacer menuCells;

    [SerializeField] float scaleKoefficient = 2.5f;
    [SerializeField] List<Transform> cells;
    [SerializeField] GameObject menu;

    List<GameObject> spawnedIcons = new List<GameObject>();
    List<SigilData> sortedDominoList = new List<SigilData>();
    HashSet<SigilType> sigilsFilters = new HashSet<SigilType>();

    //HashSet<int> numberFilters = new HashSet<int>();

    int prevAvailableCount;

    public void Init(SigilsState sigilsState)
    {
        this.sigilsState = sigilsState;

        prevAvailableCount = sigilsState.GetAllAvailable().Count;
        FillAvailable();

    }
    public void Open()
    {
        menu.SetActive(true);
        if (spawnedIcons.Count == 0) FillAvailable();
    }
    public void Close()
    {
        menu.SetActive(false);
    }
    private void Update()
    {
        if (prevAvailableCount != sigilsState.GetAllAvailable().Count) 
        {
            Debug.Log("Update");
            UpdateAvailable();
            menuCells.UpdateButtons();
            prevAvailableCount = sigilsState.GetAllAvailable().Count;
        }

    }

    //public void ApplyFilter(int num)
    //{
    //    if (numberFilters.Contains(num))
    //    {
    //        numberFilters.Remove(num);
    //    }
    //    else
    //    {
    //        numberFilters.Add(num);
    //    }
    //    UpdateAvailable();
    //}
    public void ApplyFilter(SigilType im)
    {
        if (sigilsFilters.Contains(im))
        {
            sigilsFilters.Remove(im);
        }
        else
        {
            sigilsFilters.Add(im);
        }
        UpdateAvailable();
    }

    void FillAvailable() 
    {
        if (sigilsState.HasAvailable())
        {
            int curIndx = 0;
            sortedDominoList = sigilsState.GetAllAvailable();
            sortedDominoList.Sort((a, b) => DominoManager.Instance.order[a.characteristics.sigilType].CompareTo(DominoManager.Instance.order[b.characteristics.sigilType]));
            // sortedDominoList.Sort((a, b) => a.characteristics.boneNumber.CompareTo(b.characteristics.number));

            foreach (SigilData d in sortedDominoList)
            {
                bool isTypeOk = sigilsFilters.Count == 0 || sigilsFilters.Contains(d.characteristics.sigilType);
               // bool isNumberOk = numberFilters.Count == 0 || numberFilters.Contains(d.characteristics.number);
                if (isTypeOk) // && isNumberOk)
                {
                    cells[curIndx].gameObject.SetActive(true);
                    GameObject icon = Instantiate(d.UIprefab, menu.transform);
                    icon.transform.position = cells[curIndx++].position;
                    icon.transform.localScale *= scaleKoefficient;
                    spawnedIcons.Add(icon);
                }
            }
        }

    }

    void ClearAvailable()
    {
        int curIndx = 0;
        foreach(GameObject icon in spawnedIcons)
        {
            Destroy(icon);
            cells[curIndx++].gameObject.SetActive(false);
        }
        spawnedIcons.Clear();
    }

    void UpdateAvailable()
    {
        ClearAvailable();
        FillAvailable();
    }
 

}
