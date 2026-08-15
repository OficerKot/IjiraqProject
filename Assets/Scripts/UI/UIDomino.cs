
using UnityEngine.EventSystems;
using UnityEngine;
using System;
using UnityEngine.UI;
using VContainer;


public class UIDomino : MonoBehaviour, IPointerClickHandler
{
    GameObject part1, part2;
    Domino domino;
    [SerializeField] bool clicked = false;
    public event Action<UIDomino> OnDestroyed;
    HandManager _handManager;

    public void Init(HandManager handManager, Domino domino, UIConfig config)
    {
        _handManager = handManager;
        this.domino = domino;
        domino.OnPlaced += OnDominoPlaced;

        part1 = Instantiate(config.sigil, transform);
        part2 = Instantiate(config.sigil, transform);

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

        if (_handManager.GetContent() == null || _handManager.GetContent() == domino.gameObject)
        {
            clicked = !clicked;
            if (clicked)
            {
                _handManager.Take(domino.gameObject);
                domino.PickUp();
                domino.pivot.SetActive(true);
                //blurImage.SetActive(true);
            }
            else
            {
                _handManager.Take(null);
                domino.pivot.SetActive(false);
                //blurImage.SetActive(false);
            }
        }
    }
}
