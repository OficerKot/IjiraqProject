using UnityEngine;

/// <summary>
/// Меню паузы. Приостанавливает игру при активации.
/// </summary>
public class PauseMenu : MonoBehaviour, IMenu
{
    [SerializeField] GameObject menu;

    /// <summary>
    /// Открытие меню, игра устанавливается на паузу.
    /// </summary>
    public void Open()
    {
        menu.SetActive(true);
        GameManager.Instance.Pause();
    }

    /// <summary>
    /// Закрытие меню, возообновление игры.
    /// </summary>
    public void Close()
    {
        menu.SetActive(false);
        GameManager.Instance.Pause();
    }
}
