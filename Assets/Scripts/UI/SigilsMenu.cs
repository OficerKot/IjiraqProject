using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using VContainer;
using UnityEngine.UI;

public class SigilsMenu : MonoBehaviour, IMenu
{
    SigilsState _sigilsState;
    SigilsMenuIconsPlacer _icons;
    DominoConfig _config;

    [SerializeField] float scaleKoefficient = 2.5f;
    [SerializeField] List<Transform> cells;
    [SerializeField] GameObject menu;

    List<GameObject> spawnedIcons = new List<GameObject>();
    List<SigilData> sigilsList = new List<SigilData>(); 
    HashSet<SigilData> sigilsFilters = new HashSet<SigilData>();
    HashSet<int> numbersFilters = new HashSet<int>();

    [Inject]
    public void Construct(SigilsState sigilsState, DominoConfig dominoConfig, SigilsMenuIconsPlacer icons)
    {
        _sigilsState = sigilsState;
        _sigilsState.OnUnlockedSigilsChanged += OnSigilsStateChanged;
        _config = dominoConfig;
        _icons = icons;

        FillAvailable();
        icons.UpdateButtons();
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
    private void OnSigilsStateChanged()
    {
        UpdateAvailable();
        _icons.UpdateButtons();
    }

    public void ToggleFilter(int num)
    {
        if (numbersFilters.Contains(num))
        {
            numbersFilters.Remove(num);
        }
        else
        {
            numbersFilters.Add(num);
        }
        UpdateAvailable();
    }
    public void ToggleFilter(SigilData im)
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

    void UpdateAvailable()
    {
        ClearAvailable();
        FillAvailable();
    }
    void FillAvailable() 
    {
        if (_sigilsState.GetAvailableTypes().Count > 0)
        {
            var sigils = _sigilsState.GetAvailableTypes();

            int curIndx = 0;
            sigilsList = new List<SigilData>(sigils.Keys);

            foreach (SigilData d in sigilsList)
            {
                List<int> allBoneNumbers = new List<int>(sigils[d]);
                allBoneNumbers.Sort((a,b) => a.CompareTo(b));

                bool isTypeOk = sigilsFilters.Count == 0 || sigilsFilters.Contains(d);

                foreach (int num in allBoneNumbers) {
                    if (curIndx >= cells.Count) return;
                    bool isNumberOk = numbersFilters.Count == 0 || numbersFilters.Contains(num);

                    if (isTypeOk && isNumberOk)
                    {
                        cells[curIndx].gameObject.SetActive(true);
                        GameObject icon = Instantiate(_config.UISigilPrefab, menu.transform);
                        icon.GetComponent<Image>().sprite = d.sprites.Length > 1 ? d.sprites[num-1] : d.sprites[0];

                        icon.transform.position = cells[curIndx++].position;
                        icon.transform.localScale *= scaleKoefficient;
                        spawnedIcons.Add(icon);
                    }
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

}
