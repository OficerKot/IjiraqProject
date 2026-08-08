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

    public void SetIconState(IconState res)
    {
        switch (res)
        {
            case IconState.Pressed:
                MakeDark();
                break;

            case IconState.Released:
                MakeBright();
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

public enum IconState
{
    Pressed, Released
}
