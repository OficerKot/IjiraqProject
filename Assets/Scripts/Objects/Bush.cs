using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

/// <summary>
/// Скрипт, управляющий поведением объекта Куст
/// При нажатии мышью на куст скрипт проверяет, рядом ли находится игрок, при помощи IsNearbyPlayer().
/// Если рядом, то Interact() выдаёт игроку ягоду, меняет кусту анимацию с куста с ягодами на куст без ягод и уничтожает этот компонент.
/// 
/// Дополнительная настройка: Не требуется. Установить на префаб Куст и не трогать.
/// </summary>
public class Bush : MonoBehaviour
{
    public float minDistanceToInteract;
    private SpriteAnimator sprAnim;
    private string itemID = "berry";

    private IInventory _inventory;

    [Inject]
    private void Construct(IInventory inventory)
    {
        _inventory = inventory;
    }

    private void Awake()
    {
        sprAnim = GetComponent<SpriteAnimator>();
    }
    private void OnMouseDown()
    {
        if (IsNearbyPlayer() && (_inventory.Contains(ItemsDataBase.Instance.GetItemByID(itemID)) || !_inventory.IsFull()) )
        {
            Interact();
        }
    }
    bool IsNearbyPlayer()
    {
        Vector2 playerPos = GameObject.FindWithTag("Player").transform.position;
        Vector2 thisPos = transform.position;

        float dist = Mathf.Pow(playerPos.x - thisPos.x, 2) + Mathf.Pow(playerPos.y - thisPos.y, 2);
        return dist <= Mathf.Pow(minDistanceToInteract, 2);
    }

    void Interact()
    {
        _inventory.AddItem(ItemsDataBase.Instance.GetItemByID(itemID));
        sprAnim.ForcePlay("BushEmpty");
        Destroy(this);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, Vector3.forward, minDistanceToInteract); 
        GUIStyle style = new GUIStyle();
        Handles.Label(transform.position + Vector3.right * (minDistanceToInteract + 0.5f),
                     $"Radius: {minDistanceToInteract}", style);
    }
#endif
}
