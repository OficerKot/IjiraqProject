using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

public class Berry : MonoBehaviour, IPointerClickHandler
{
    private UIPickableIcon berryUIPI;
    public ItemData berry;

    private IInventory _inventory;

    [Inject]
    private void Construct(IInventory inventory)
    {
        _inventory = inventory;
    }
    void Awake()
    {
        berryUIPI = this.GetComponent<UIPickableIcon>();
        berryUIPI.SetDisposable();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Hunger.Instance.CallHungerUp();
        _inventory.RemoveItem(berry);
    }
}
