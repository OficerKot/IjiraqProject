using System.Collections.Generic;
using UnityEngine;

public class UIManager : PauseBehaviour
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
    public void Init(SigilsState sigilsState)
    {
        sigilsMenu.Init(sigilsState);
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