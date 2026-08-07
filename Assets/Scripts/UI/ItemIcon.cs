using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

public class ItemIcon : MonoBehaviour, IPointerClickHandler
{
    public ItemData data { get; private set; }
    Image img;
    public event Action<ItemIcon> OnClick;

    public void Init(ItemData data)
    {
        this.data = data;

        img = GetComponent<Image>();
        img.sprite = data.sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick.Invoke(this);
    }

    public void SetIconState(ToggleResult res)
    {
        switch (res)
        {
            case ToggleResult.Taken:
                MakeDark();
                break;

            case ToggleResult.Released:
                MakeBright();
                break;

            case ToggleResult.Failed:
                break;
        }
    }
    public void MakeDark()
    {
        img.color = Color.darkGray;
    }

    public void MakeBright()
    {
        img.color = Color.white;
    }

}
