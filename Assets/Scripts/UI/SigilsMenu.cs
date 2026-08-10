using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using VContainer;

public class SigilsMenu : MonoBehaviour, IMenu
{
    SigilsState _sigilsState;
    SigilsMenuIconsPlacer menuCells;

    [SerializeField] float scaleKoefficient = 2.5f;
    [SerializeField] List<Transform> cells;
    [SerializeField] GameObject menu;

    List<GameObject> spawnedIcons = new List<GameObject>();
    List<SigilData> sortedDominoList = new List<SigilData>();
    HashSet<Sprite> sigilsFilters = new HashSet<Sprite>();

    //HashSet<int> numberFilters = new HashSet<int>();

    int prevAvailableCount;

    [Inject]
    public void Construct(SigilsState sigilsState)
    {
        _sigilsState = sigilsState;

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
        if (prevAvailableCount != _sigilsState.GetAllAvailable().Count) 
        {
            Debug.Log("Update");
            UpdateAvailable();
            menuCells.UpdateButtons();
            prevAvailableCount = _sigilsState.GetAllAvailable().Count;
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
    public void ApplyFilter(Sprite im)
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
        if (_sigilsState.HasAvailable())
        {
            int curIndx = 0;
            sortedDominoList = _sigilsState.GetAllAvailable();
            sortedDominoList.Sort((a, b) => DominoManager.Instance.order[a.characteristics.sigilType].CompareTo(DominoManager.Instance.order[b.characteristics.sigilType]));

            foreach (SigilData d in sortedDominoList)
            {
                bool isTypeOk = sigilsFilters.Count == 0 || sigilsFilters.Contains(d.sprites[0]);
                if (isTypeOk)
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
