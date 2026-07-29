
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

/// <summary>
/// Позиция кнопки на кольце крафта для корректного отображения кнопки и описания предмета при наведении курсора мыши.
/// </summary>
public enum Position
{
    left_down, left_up, right_down, right_up
}

/// <summary>
/// Категория меню крафта, в которой находится данная кнопка
/// </summary>
public enum Category
{
    blue, green, red
}

/// <summary>
/// Кнопка создания предмета в меню крафта. Отображает информацию о рецепте при наведении, позволяет создавать и добавлять
/// предметы в инвентарь.
/// </summary>
public class ItemSpawnerButton : Notificationable, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData objectToSpawn;
    public GameObject craftPanelPrefab;
    public string description;
    public Category category;
    public Position pos;
    Vector3 offset;
    GameObject infoPanel;
    [SerializeField] GameObject AvailableImage; //в будущем нужно свести к одной переменной и работать с яркостью изображения
    [SerializeField] GameObject notAvailableImage;

    private ICraftService _craftService;
    [Inject]
    private void Construct(ICraftService craftService)
    {
        _craftService = craftService;
    }


    private void Start()
    {
        switch (pos)     //Подбор отступов для всплывающих окон при наведении на кнопку.
        {
            case Position.left_down:
                offset = new Vector3(-40, -45, 0);
                break;
            case Position.left_up:
                offset = new Vector3(-40, 45, 0);
                break;
            case Position.right_down:
                offset = new Vector3(40, -45, 0);
                break;
            case Position.right_up:
                offset = new Vector3(40, 45, 0);
                break;
        }
    }

    public void SetAvailability(bool isAvailable)
    {
        if (isAvailable)
        {
            AvailableImage.SetActive(true);
            notAvailableImage.SetActive(false);
        }
        else
        {
            AvailableImage.SetActive(false);
            notAvailableImage.SetActive(true);
        }
    }

    /// <summary>
    /// При наведении курсора показывает панель с информацией о предмете.
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        infoPanel = Instantiate(craftPanelPrefab, transform.position, transform.rotation, (GameObject.Find("Craft").transform));
        RectTransform rect = infoPanel.GetComponent<RectTransform>();
        rect.localPosition += offset;
        infoPanel.GetComponent<RecipeInfo>().text.text = description;
        AddItemsForCraft();
    }

    /// <summary>
    /// При уходе курсора скрывает панель с информацией о предмете.
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(infoPanel);
        infoPanel = null;
    }

    private void OnDisable()
    {
        if (infoPanel)
        {
            Destroy(infoPanel);
            infoPanel = null;
        }
    }

    /// <summary>
    /// Добавляет иконки необходимых предметов для крафта предмета на панель.
    /// </summary>
    void AddItemsForCraft()
    {
        int cellIndx = 0;
        List<GameObject> cells = infoPanel.GetComponent<RecipeInfo>().cells;
        foreach (ItemData obj in objectToSpawn.itemsForCraft)
        {
            GameObject icon = Instantiate(obj.UIprefab, cells[cellIndx].transform);
            icon.transform.position = cells[cellIndx].transform.position;
            Destroy(icon.GetComponent<UIPickableIcon>());
            cellIndx++;
        }
    }
}