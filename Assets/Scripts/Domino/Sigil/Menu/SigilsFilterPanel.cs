using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SigilsFilterPanel
{
    FilterState<SigilData> _state = new FilterState<SigilData>();
    List<SigilFilterButton> spawnedButtons = new List<SigilFilterButton>();

    UICellsPlacer _placer;
    public event Action OnChanged;
    UIConfig _config;

    Vector2 _pos;
    float _defaultSpacing;
    int _maxElementsBeforeCompression;

    public SigilsFilterPanel(Transform parent, UIConfig config, Vector2 pos, float defaultSpacing, int maxElemBeforeCompression)
    {
        _placer = new UICellsPlacer(config.sigilFilterButton, parent);
        _config = config;
        _pos = pos;
        _defaultSpacing = defaultSpacing;
        _maxElementsBeforeCompression = maxElemBeforeCompression;
    }

    public Dictionary<SigilData, HashSet<int>> ApplyFilters(Dictionary<SigilData, HashSet<int>> sigils)
    {
        Dictionary <SigilData, HashSet<int>> res = new Dictionary<SigilData, HashSet<int>>(sigils);

        if (_state.HasAppliedFilters())
        {
            foreach (var sigil in sigils)
            {
                if (!_state.IsApplied(sigil.Key))
                {
                    res.Remove(sigil.Key);
                }
            }
        }

        return res;
    }
    public void UpdateButtons(List<SigilData> sigils)
    {
        ClearButtons();

        float offsetX = CountOffsetX(sigils.Count, _maxElementsBeforeCompression, _defaultSpacing);

        SpawnButtons(sigils, _pos, offsetX);
    }
    float CountOffsetX(int elementsCnt, int maxElementsBeforeCompression, float defaultOffset)
    {
        float offsetX;
        if (elementsCnt <= maxElementsBeforeCompression)
        {
            offsetX = defaultOffset;
        }
        else
        {
            float k = (float)maxElementsBeforeCompression / elementsCnt;
            offsetX = defaultOffset * k;
        }

        return offsetX;
    }
    void SpawnButtons(List<SigilData> sigils, Vector2 pos, float spacing)
    { 
        List<GameObject> buttons = _placer.CreateRow(sigils.Count, spacing, pos);

        int indx = 0;
        foreach (SigilData sigil in sigils)
        {
            {
                SigilFilterButton filterButton = buttons[indx++].AddComponent<SigilFilterButton>();
                filterButton.Init(sigil, _config.sigil);
                filterButton.OnFilterToggled += OnToggled;

                spawnedButtons.Add(filterButton);
            }
        }
    }

    void ClearButtons()
    {
        for (int i = 0; i < spawnedButtons.Count; i++)
        {
            GameObject.Destroy(spawnedButtons[i].gameObject);
        }
         spawnedButtons.Clear();
     }

    public void OnToggled(SigilData sigil)
    {
        _state.Toggle(sigil);
        OnChanged?.Invoke();
    }
}
