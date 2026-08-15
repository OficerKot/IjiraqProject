using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Кнопки фильтрации в меню сигилов. При нажатии в меню остаются только сигилы с соответствующим изображением
/// </summary>
public class SigilFilterButton : MonoBehaviour, IPointerClickHandler
{
    bool clicked = false;
    SigilData _sigil;
    public event Action<SigilData> OnFilterToggled;
    Image sigilIcon;
    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle();
    }

    public void Init(SigilData sigil, GameObject uiSigilPrefab)
    {
        _sigil = sigil;
        SetSigilIcon(sigil.sprites[0], uiSigilPrefab);
    }

    void SetSigilIcon(Sprite icon, GameObject sigilPrefab)
    {
        sigilIcon = GameObject.Instantiate(sigilPrefab, transform).GetComponent<Image>();
        sigilIcon.sprite = icon;

        sigilIcon.transform.SetAsFirstSibling();
        sigilIcon.transform.localPosition = Vector3.zero;
        sigilIcon.GetComponent<Image>().raycastTarget = false;
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

        OnFilterToggled?.Invoke(_sigil);
    }

    void MakeBright()
    {
        GetComponent<Image>().color = Color.white;
        sigilIcon.color = Color.white;
    }

    void MakeDark()
    {
        GetComponent<Image>().color = Color.darkGray;
        sigilIcon.color = Color.darkGray;
    }

}
