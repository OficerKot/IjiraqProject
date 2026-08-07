using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

public class Berry : MonoBehaviour, IPointerClickHandler
{
    private ItemIcon berryUIPI;
    public ItemData berry;

    private IInventory _inventory;

    [Inject]
    private void Construct(IInventory inventory)
    {
        _inventory = inventory;
    }
    void Awake()
    {
        berryUIPI = this.GetComponent<ItemIcon>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Hunger.Instance.CallHungerUp();
        _inventory.RemoveItem(berry);
    }
}
