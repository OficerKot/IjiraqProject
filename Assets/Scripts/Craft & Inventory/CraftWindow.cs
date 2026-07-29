using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;
using UnityEngine.UI;
using VContainer;

/// <summary>
/// Окно крафта предметов с системой рецептов и категорий.
/// </summary>
public class CraftWindow : MonoBehaviour, IMenu
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject curWindow;
    [SerializeField] List<MenuButton> categoryButtons;
    [SerializeField] List<ItemSpawnerButton> spawnerButtons;

    private ICraftService _craftService;
    [Inject]
    private void Construct(ICraftService craftService)
    {
        _craftService = craftService;
        _craftService.OnRecipesUpdated += UpdateButtons;
    }   

    private void Start()
    {
        foreach (MenuButton b in categoryButtons)
        {
            b.GetComponent<Button>().onClick.AddListener(() => OpenCategoryMenu(b.GetComponent<MenuButton>().window));
        }
        foreach (ItemSpawnerButton b in spawnerButtons)
        {
            b.GetComponent<Button>().onClick.AddListener(() => _craftService.Craft(b.GetComponent<ItemSpawnerButton>().objectToSpawn));
        }
    }

    /// <summary>
    /// Закрывает окно крафта.
    /// </summary>
    public void Close()
    {
        if (curWindow)
        {
            curWindow.SetActive(false);
            curWindow = null;
        }
        menu.SetActive(false);
    }

    /// <summary>
    /// Открывает окно крафта в позиции курсора мыши.
    /// </summary>
    public void Open()
    {
        if (curWindow)
        {
            menu.transform.position = curWindow.transform.position;
            curWindow.SetActive(false);
            curWindow = null;
        }
        else
        {
            menu.transform.position = Input.mousePosition;
        }
        menu.SetActive(true);
    }

    /// <summary>
    /// Открывает меню выбранной категории.
    /// </summary>
    /// <param name="g">Окно категории для открытия.</param>
    public void OpenCategoryMenu(GameObject g)
    {
        curWindow = g;
        menu.SetActive(false);
        curWindow.SetActive(true);
        curWindow.transform.position = menu.transform.position;
    }

    public void AddNewRecipe(ItemSpawnerButton b)
    {
        b.GetComponent<Notificationable>().ShowNotification();

        MenuButton categoryButton = categoryButtons.Find(but => but.category == b.category);
        categoryButton.ShowNotification();

        AudioManager.Play(SoundType.NewReciepe); // вынести
    }

    public void UpdateButtons()
    {
        foreach (ItemSpawnerButton b in spawnerButtons)
        {
            ItemData item = b.objectToSpawn;
            if (_craftService.CanCraft(item))
            {
                b.gameObject.SetActive(true);
                b.SetAvailability(true);
            }
            else if (_craftService.IsExplored(item))
            {
                b.gameObject.SetActive(true);
                b.SetAvailability(false);
            }
            else
            {
                b.gameObject.SetActive(false);
            }


        }
    }
    
}