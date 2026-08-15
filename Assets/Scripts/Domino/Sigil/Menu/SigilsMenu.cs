using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using VContainer;
using UnityEngine.UI;

public class SigilsMenu : MonoBehaviour, IMenu
{
    SigilsIconsPanel playerSigils;
    SigilsFilterPanel sigilsFilters;
    NumbersFilterPanel numberFilters;

    [Header("Выбор окна меню")]
    [SerializeField] GameObject menuWindow;

    [Header("Настройки отображения фильтров очков")]
    [SerializeField] Vector2 numberFiltersLocalPos;
    [SerializeField] float numberFiltersSpacing;
    [SerializeField] bool isNumbersFiltersAtCenter;

    [Header("Настройки отображения фильтров сигилов")]
    [SerializeField] Vector2 sigilsFiltersLocalPos;
    [SerializeField] float sigilsFiltersSpacing;
    [SerializeField] int sigilFiltMaxElemBeforeCompression;
    [SerializeField] bool isSigilsFiltersAtCenter;

    [Header("Настройки отображения доступных сигилов")]
    [SerializeField] bool isSigilsGridAtCenter;
    [SerializeField] Vector2 gridSpacing;
    [SerializeField] Vector2 gridPos;
    [SerializeField] int gridWidth;
    [SerializeField] int gridHeight;
    

    SigilsState _sigilsState;
    UIConfig _configUI;

    [Inject]
    public void Construct(SigilsState sigilsState, UIConfig uIConfig)
    {
        _sigilsState = sigilsState;
        _sigilsState.OnUnlockedSigilsChanged += OnSigilsStateChanged;
        _configUI = uIConfig;

        CountPositions();

        playerSigils = new SigilsIconsPanel(_configUI, menuWindow.transform, gridPos, gridSpacing, gridWidth, gridHeight);

        sigilsFilters = new SigilsFilterPanel(
            menuWindow.transform, _configUI, sigilsFiltersLocalPos, sigilsFiltersSpacing, sigilFiltMaxElemBeforeCompression);
        sigilsFilters.OnChanged += UpdateMenuContent;

        numberFilters = new NumbersFilterPanel(menuWindow.transform, _configUI);
        numberFilters.OnChanged += UpdateMenuContent;

        OnSigilsStateChanged();
    }

    void CountPositions()
    {
        if (isNumbersFiltersAtCenter) numberFiltersLocalPos.x = CountStartPosXForCenter(_configUI.numberFilterButton, _configUI.numberFiltersSprites.Count, numberFiltersSpacing);

        if(isSigilsFiltersAtCenter) sigilsFiltersLocalPos.x = CountStartPosXForCenter(_configUI.sigilFilterButton, _sigilsState.GetAvailableTypes().Count, sigilsFiltersSpacing);

        if (isSigilsGridAtCenter) gridPos.x = CountStartPosXForCenter(_configUI.sigilCell, gridWidth, gridSpacing.x);
    }
    public void Open()
    {
        menuWindow.SetActive(true);
    }
    public void Close()
    {
        menuWindow.SetActive(false);
    }
    public void OnSigilsStateChanged()
    {
        sigilsFilters.UpdateButtons(_sigilsState.GetAvailableTypes());
        numberFilters.SpawnButtons(numberFiltersLocalPos, numberFiltersSpacing);

        UpdateMenuContent();
    }

    private void UpdateMenuContent()
    {
        Dictionary<SigilData, HashSet<int>> visibleSigils = ApplyFilters();
        playerSigils.Show(visibleSigils);
    }

    //Если фильтров станет больше, панели можно хранить как список <IFilterPanel>, и в цикле делать Apply
    Dictionary<SigilData, HashSet<int>> ApplyFilters()
    {
        Dictionary<SigilData, HashSet<int>> result = sigilsFilters.ApplyFilters(_sigilsState.GetAvailableVariants());
        result = numberFilters.ApplyFilters(result);

        return result;
    }

    float CountStartPosXForCenter(GameObject cellPrefab, int elementsCnt, float spacing)
    {
        float cellWidth = cellPrefab.GetComponent<RectTransform>().rect.width;
        float rowWidth =
            elementsCnt * cellWidth +
            (elementsCnt - 1) * spacing;

        float startX =
            -rowWidth / 2f +
            cellWidth / 2f;

        return startX;
    }
}
