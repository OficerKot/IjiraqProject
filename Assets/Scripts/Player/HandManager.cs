using UnityEngine;
using VContainer;
public class HandManager 
{
    GameObject handContent = null;

    public bool IsHandFree()
    {
        return handContent == null;
    }

    public bool IsHoldingItem(ItemData data)
    {
        if (handContent == null)
            return false;

        if (!handContent.TryGetComponent<Item>(out var item))
            return false;

        return item.data == data;
    }

    public GameObject GetContent()
    {
        return handContent;
    }

    public void Release() 
    {
        if (!handContent) return;
        handContent = null;
    }

    public void Take(GameObject obj)
    {
        handContent = obj;
    }
  
}