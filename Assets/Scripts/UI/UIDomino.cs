
using UnityEngine.EventSystems;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;


public class UIDomino : MonoBehaviour, IPointerClickHandler
{
    GameObject part1, part2;

    [SerializeField] bool clicked = false;

    public void Init(Domino domino, DominoConfig config)
    {
        part1 = Instantiate(config.UISigilPrefab, transform);
        part2 = Instantiate(config.UISigilPrefab, transform);

        part1.transform.SetParent(transform);
        part2.transform.SetParent(transform);

        SetSprites(domino.part1, domino.part2);
        SetLocations();
       
    }

    void SetSprites(DominoPart p1, DominoPart p2)
    {
        Image part1Img = part1.GetComponent<Image>();
        Image part2Img = part2.GetComponent<Image>();

        part1Img.sprite = p1.sigilVariantData.Sprite;
        part2Img.sprite = p2.sigilVariantData.Sprite;
    }

    void SetLocations()
    {
        var rectTransform = GetComponent<RectTransform>();
        var height = rectTransform.rect.height;

        var part1Rect = part1.GetComponent<RectTransform>();
        var part2Rect = part2.GetComponent<RectTransform>();

        part1Rect.localPosition = new Vector2(0, height / 4);
        part2Rect.localPosition = new Vector2(0, -height / 4);
    }
    public void OnPointerClick(PointerEventData eventData)
    {

        if (HandManager.Instance.WhatInHand() == null || HandManager.Instance.WhatInHand() == gameObject)
        {
            clicked = !clicked;
            if (clicked)
            {
                HandManager.Instance.PutInHand(gameObject);
                //blurImage.SetActive(true);
            }
            else
            {
                HandManager.Instance.PutInHand(null);
                //blurImage.SetActive(false);
            }
        }
    }
}
