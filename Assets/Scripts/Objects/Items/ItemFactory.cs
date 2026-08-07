using UnityEngine;
using VContainer;

public class ItemFactory
{
    IObjectResolver _resolver;

    [Inject]
    void Construct(IObjectResolver resolver)
    {
        _resolver = resolver;
    }
    public Item CreateItem(ItemData data)
    {
        Item item = GameObject.Instantiate(data.prefab).GetComponent<Item>();

        item.SetIsPlaced(false);
        _resolver.Inject(item);

        return item;
    }
}
