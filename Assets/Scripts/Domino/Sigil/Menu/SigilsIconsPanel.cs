using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class SigilsIconsPanel
{
    Vector2 pos;
    Vector2 spacing;
    int width, height;

    List<GameObject> cells; 
    List<GameObject> spawnedIcons= new List<GameObject>();

    UICellsPlacer _placer;
    Transform _parent;
    UIConfig _config;

    
    public SigilsIconsPanel (UIConfig config, Transform parent, Vector2 pos, Vector2 spacing, int width, int height) //без комментариев...
    {
        _config = config;
        _parent = parent;

        this.pos = pos;
        this.spacing = spacing;
        this.width = width;
        this.height = height;

        CreateCellsForSigils();
    }
    void CreateCellsForSigils()
    {
        _placer = new UICellsPlacer(_config.sigilCell, _parent);
        cells = _placer.CreateGrid(width, height, pos, spacing);
        DeactivateCells();
    }

    void DeactivateCells()
    {
        foreach (var cell in cells)
        {
            cell.SetActive(false);
        }
    }

    public void Show(Dictionary<SigilData, HashSet<int>> sigils)
    {
        Clear();

        int curIndx = 0;
        foreach (SigilData d in sigils.Keys)
        {
            List<int> sortedBoneNumbers = new List<int>(sigils[d]);
            sortedBoneNumbers.Sort((a, b) => a.CompareTo(b));

            foreach (int num in sortedBoneNumbers)
            {
                if (curIndx >= cells.Count) return;

                cells[curIndx].gameObject.SetActive(true);

                GameObject icon = GameObject.Instantiate(_config.sigil, cells[curIndx].transform);
                icon.GetComponent<RectTransform>().localPosition = Vector2.zero;
                icon.GetComponent<Image>().sprite = d.sprites.Length > 1 ? d.sprites[num - 1] : d.sprites[0];
                

                spawnedIcons.Add(icon);
                curIndx++;
            }
        }
    }

    void Clear()
    {
        if (spawnedIcons.Count == 0) return;

        int curIndx = 0;
        foreach (GameObject icon in spawnedIcons)
        {
            GameObject.Destroy(icon);
            cells[curIndx++].gameObject.SetActive(false);
        }
        spawnedIcons.Clear();
    }
}
