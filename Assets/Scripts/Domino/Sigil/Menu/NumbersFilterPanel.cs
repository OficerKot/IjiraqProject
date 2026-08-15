using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NumbersFilterPanel
{
    FilterState<int> _state = new FilterState<int>();
    List<NumFilterButton> spawnedButtons = new List<NumFilterButton>();
    public event Action OnChanged;

    UIConfig _uiConfig;
    UICellsPlacer _placer;
    public NumbersFilterPanel(Transform parent, UIConfig uIConfig)
    {
        _uiConfig = uIConfig;
        _placer = new UICellsPlacer(uIConfig.numberFilterButton, parent);

    }

    public Dictionary<SigilData, HashSet<int>> ApplyFilters(Dictionary<SigilData, HashSet<int>> sigils)
    {
        Dictionary<SigilData, HashSet<int>> res = sigils.ToDictionary(
        x => x.Key,
        x => new HashSet<int>(x.Value)
    );

        if (_state.HasAppliedFilters())
        {
            foreach (var sigil in sigils)
            {
                foreach (var num in sigil.Value)
                {
                    if (!_state.IsApplied(num))
                    {
                        res[sigil.Key].Remove(num);
                    }
                }
            }
        }

        return res;
    }
    public void SpawnButtons(Vector2 pos, float spacing)
    {
        int buttonsCnt = _uiConfig.numberFiltersSprites.Count;
        List<GameObject> buttons = _placer.CreateRow(buttonsCnt, spacing, pos);

        for (int num = 0; num < buttonsCnt; num++)
        {
            {
                NumFilterButton filterButton = buttons[num].AddComponent<NumFilterButton>();
                filterButton.Init(num, _uiConfig.numberFiltersSprites);
                filterButton.OnFilterToggled += OnToggled;

                spawnedButtons.Add(filterButton);

            }
        }
    }

    void OnToggled(int num)
    {
        _state.Toggle(num);
        OnChanged?.Invoke();
    }

}
