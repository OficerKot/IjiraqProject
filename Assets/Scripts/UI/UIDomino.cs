
using UnityEngine.EventSystems;
using UnityEngine;
using Unity.VisualScripting;


public class UIDomino : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject blurImage;
    
    SigilData part1, part2;
    GameObject part1UI, part2UI;
    
 
    [SerializeField] float offsetY = 0;

    [SerializeField] bool clicked = false;
    void Start()
    {
        blurImage.SetActive(false);
        blurImage.transform.SetAsLastSibling();
    }


    public void OnPointerClick(PointerEventData eventData)
    {

        if (GameManager.Instance.WhatInHand() == null || GameManager.Instance.WhatInHand() == gameObject)
        {
            clicked = !clicked;
            if (clicked)
            {
                GameManager.Instance.PutInHand(gameObject);
                blurImage.SetActive(true);
            }
            else
            {
                GameManager.Instance.PutInHand(null);
                blurImage.SetActive(false);
            }
        }
    }
}
