using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Кнопка фильтрации по номерам в меню сигилов. При нажатии в меню остаются только сигилы с соответствующим номером.
/// </summary>
public class NumFilterButton : MonoBehaviour, IPointerClickHandler
{
    bool clicked = false;
    int _num;
    public event Action<int> OnFilterToggled;

    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle();
    }
    public void Init(int num, List<Sprite> sprites)
    {
        if (num < 0 || num >= sprites.Count)
        {
            Debug.LogError(
                $"Cannot initialize NumFilterButton: " +
                $"number {num} has no corresponding sprite.",
                this);

            return;
        }

        _num = num;
        GetComponent<Image>().sprite = sprites[num];
    }
    void Toggle()
    {
        if (clicked)
        {
            clicked = false;
            MakeBright();
        }
        else
        {
            clicked = true;
            MakeDark();
        }

        OnFilterToggled?.Invoke(_num);
    }

    void MakeBright()
    {
        GetComponent<Image>().color = Color.white;
    }

    void MakeDark()
    {
        GetComponent<Image>().color = Color.darkGray;
    }
}


