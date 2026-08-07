using TMPro;
using UnityEngine;

public class UIInventoryCell : MonoBehaviour
{
    [SerializeField] public ItemIcon icon { get; private set; }
    [SerializeField] int itemCount;
    [SerializeField] TextMeshProUGUI counterText;

    private void Start()
    {
        if (icon == null)
        {
            ClearCounter();
        }
    }

    public void PutIcon(ItemIcon i)
    {
        icon = i;
        if (i == null)
        {
            ClearCounter();
            return;
        }
        SetCounter(1);

        i.transform.position = transform.position;
    }
    public void RemoveItem()
    {
        Destroy(icon);
        icon = null;
        ClearCounter();
    }
    public void SetCounter(int i)
    {
        itemCount = i;
        counterText.text = itemCount.ToString();
    }

    void ClearCounter()
    {
        itemCount = 0;
        counterText.text = "";
       
    }


}
