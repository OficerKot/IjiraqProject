
using UnityEngine.EventSystems;
using UnityEngine;
using System;
using UnityEngine.UI;


public class UIDomino : MonoBehaviour, IPointerClickHandler
{
    GameObject part1, part2;
    Domino domino;
    [SerializeField] bool clicked = false;
    public event Action<UIDomino> OnDestroyed;
    public void Init(Domino domino, DominoConfig config)
    {
        this.domino = domino;
        domino.OnPlaced += OnDominoPlaced;

        part1 = Instantiate(config.UISigilPrefab, transform);
        part2 = Instantiate(config.UISigilPrefab, transform);

        part1.transform.SetParent(transform);
        part2.transform.SetParent(transform);

        SetSprites(domino.part1, domino.part2);
        SetLocations();
       
    }

    void OnDominoPlaced(Domino d)
    {
        d.OnPlaced -= OnDominoPlaced;
        OnDestroyed.Invoke(this);
        Destroy(gameObject);
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

        if (HandManager.Instance.WhatInHand() == null || HandManager.Instance.WhatInHand() == domino.gameObject)
        {
            clicked = !clicked;
            if (clicked)
            {
                HandManager.Instance.PutInHand(domino.gameObject);
                domino.PickUp();
                domino.pivot.SetActive(true);
                //blurImage.SetActive(true);
            }
            else
            {
                HandManager.Instance.PutInHand(null);
                domino.pivot.SetActive(false);
                //blurImage.SetActive(false);
            }
        }
    }
}
