using System;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Управляет зависимостями и игровым процессом: начало, перезагрузка, окончание игры, выход из приложения. 
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    /// <summary>
    /// Установка игры на паузу или снятие с паузы.
    /// </summary>
    public static event Action<bool> OnGamePaused;
    bool paused = false;
    public bool dominoPlaced { get; private set; } = false;
    public bool gameEnd = false;

    [Header("Меню")]
    [SerializeField] UIManager uiManager;

    [Header("Состояние игры")]
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject winScreen;

    [Header("Сигилы игрока")]
    [SerializeField] SigilsState sigilsState;

    [Header("Генерация домино")]
    [SerializeField] public const int DOMINO_CNT = 5;
    [SerializeField] GameObject dominoPrefab;
    [SerializeField] DominoPool dominoPool;
    [SerializeField] UISelectionPanel uiSelectionPanel;
    DominoFactory dominoFactory;


    [Header("Внешний вид домино")]
    [SerializeField] DominoConfig config;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            Domino.OnAnyDominoPlaced += OnAnyDominoPlaced;

            sigilsState.Init(DominoManager.Instance);
            uiManager.Init(sigilsState);

            dominoFactory = new DominoFactory();

            dominoFactory.Init(sigilsState, config);
            dominoPool.Init(dominoFactory);
            uiSelectionPanel.Init(dominoPool, config);
            
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        gameEnd = false;
        dominoPlaced = false;
        winScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        UpdateDominoSet();
    }

    public void OnAnyDominoPlaced(Domino d)
    {
        if(!dominoPlaced) dominoPlaced = true;
    }

    // Скорее всего потом куда то вынести
    public void UpdateDominoSet()
    {
        dominoPool.UpdateDominoSet(DOMINO_CNT);
        uiSelectionPanel.UpdatePanel();
    }

    /// <summary>
    /// Перезагрузка уровня.
    /// </summary>
    public void Restart()
    {
        Pause();
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Выход из игры, закрытие приложения.
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Пауза.
    /// </summary>
    public void Pause()
    {
        paused = !paused; 
        Time.timeScale = paused ? 0f : 1f;
        SetGameOnPause(paused);
    }

    /// <summary>
    /// Активация события паузы.
    /// </summary>
    /// <param name="val">true - игра приостановится, false - игра возообновится</param>
    public void SetGameOnPause(bool val)
    {
        OnGamePaused?.Invoke(val);
    }

    /// <summary>
    /// Окончание игры с проигрышем.
    /// </summary>
    public void Loose()
    {
        AudioManager.Play(SoundType.Loose);
        gameOverScreen.SetActive(true);
        gameEnd = true;
        Pause();
    }
    /// <summary>
    /// Окончание игры с выигрышем.
    /// </summary>
    public void Win()
    {
        AudioManager.Play(SoundType.Win);
        winScreen.SetActive(true);
        Pause();
    }
}
