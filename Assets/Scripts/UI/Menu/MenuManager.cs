using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MenuManager : PauseBehaviour
{
    IMenu openedMenu = null;
    bool isActive = true;

    [Header("Ёкраны меню")]
    [SerializeField] SigilsMenu sigilsMenu;
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] CraftWindow craftMenu;

    [Header(" нопки управлени€")]
    [SerializeField] KeyCode sigilsKey;
    [SerializeField] KeyCode pauseKey;
    [SerializeField] KeyCode craftKey;

    IObjectResolver _resolver;
    [Inject]
    private void Construct(IObjectResolver resolver)
    {
        _resolver = resolver;
        _resolver.Inject(sigilsMenu);
    }
  
    void Update()
    {
        if (Input.GetKeyDown(sigilsKey))
        {
            ToggleMenu(sigilsMenu);
        }
        if(Input.GetKeyDown(craftKey))
        {
            ToggleMenu(craftMenu);
        }
        if(Input.GetKeyDown(pauseKey))
        {
            ToggleMenu(pauseMenu);
        }
    }
    public override void OnGamePaused(bool isGamePaused)
    {
        isActive = !isGamePaused;
    }

    public void CloseMenu()
    {
        openedMenu.Close();
        openedMenu = null;
    }
    public void ToggleMenu(IMenu menu)
    {
        if (!isActive && GameManager.Instance.gameEnd) return;
        if (openedMenu == menu)
        {
            menu.Close();
            openedMenu = null;
        }
        else if (openedMenu == null)
        {
            menu.Open();
            openedMenu = menu;
        }
    }
}