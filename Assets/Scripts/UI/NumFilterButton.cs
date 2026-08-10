using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Кнопка фильтрации по номерам в меню сигилов. При нажатии в меню остаются только сигилы с соответствующим номером.
/// </summary>
public class NumFilterButton : MonoBehaviour
{
    SigilsMenu menu;
    Image imageComponent;
    bool clicked;
    public int number;
    Button b;
    void Start()
    {
        menu = GetComponentInParent<SigilsMenu>();

        clicked = false;
        b = GetComponent<Button>();
        imageComponent = GetComponent<Image>();
        b.onClick.AddListener(ApplyFilter);
    }

    /// <summary>
    /// Нажатие/отпускание кнопки и последующая активация/деактивация фильтра в SigilsMenu.
    /// </summary>
    void ApplyFilter()
    {
        menu.ToggleFilter(number); 
        if (clicked)
        {
            clicked = false;
            imageComponent.color = Color.white;
        }
        else
        {
            clicked = true;
            imageComponent.color = Color.gray6;
        }

    }
}


