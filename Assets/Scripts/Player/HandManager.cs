using UnityEngine;
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

    public ToggleResult ToggleItem(ItemData data)
    {
        if (!handContent)
        {
            //TODO: Создать
            //TODO: Положить в руку
            Debug.Log("Taken");
            return ToggleResult.Taken;
        }
        if(handContent.TryGetComponent<Item>(out var itemComp) && itemComp.data == data)
        {
            Release();
            return ToggleResult.Released;
        }

        return ToggleResult.Failed;
    }
    public GameObject GetContent()
    {
        return handContent;
    }

    public void Release() {
        handContent = null;
    }

    public void Take(GameObject obj)
    {
        handContent = obj;
    }


    
}

public enum ToggleResult
{
    Taken, Released, Failed
}