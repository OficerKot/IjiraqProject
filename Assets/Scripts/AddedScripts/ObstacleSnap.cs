using UnityEngine;

/// <summary>
/// Скрипт, фиксирующий создаваемые на старте предметы/препятствия в клетках.
/// При появлении предмета/препятствия в мире, скрипт проверяет, чем является появившийся объект. 
/// Если предмет - кладёт его в клетку методом PutInCell()
/// </summary>
public class ObstacleSnap : MonoBehaviour
{
    [SerializeField] public Cell curCell;
    [SerializeField] IInteractable obstacle;
    [SerializeField] public bool spawned = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        { 
            curCell = other.GetComponent<Cell>();
            obstacle = GetComponent<IInteractable>();
            if (obstacle != null)
            {
                if (spawned == true)
                {
                    obstacle.PutInCell(curCell);
                }
            }
            //
            //else
            //{
            //    curCell.SetFree(false);
            //}
        }
    }
}
