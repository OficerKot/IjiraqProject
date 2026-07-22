using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Управляет игровым процессом: начало, перезагрузка, окончание игры, выход из приложения. 
/// Хранит в себе объект, находящийся в руке игрока (в будущем будет вынесено в отдельный скрипт)
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    /// <summary>
    /// Событие, активируемое при установке игры на паузу или снятии с паузы.
    /// </summary>
    public static event Action<bool> OnGamePaused;
    bool paused = false;
    public bool gameEnd = false;

    [Header("Меню")]
    [SerializeField] UIManager uiManager;

    [Header("Состояние игры")]
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject winScreen;

    [Header("Генерация домино")]
    [SerializeField] GameObject dominoPrefab;
    [SerializeField] SigilsState sigilsState;
    DominoFactory dominoFactory;

    
    GameObject inHand = null; // вынести
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            sigilsState.Init(DominoManager.Instance);

            dominoFactory = new DominoFactory();
            dominoFactory.Init(sigilsState, dominoPrefab);

            uiManager.Init(sigilsState);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        gameEnd = false;
        winScreen.SetActive(false);
        gameOverScreen.SetActive(false);

    }


    /// <summary>
    /// Перезагрузка уровня
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

    //Вынести!-------------------------------------------

    public bool IsHandFree()
    {
        return inHand = null;
    }

    public GameObject WhatInHand()
    {
        return inHand;
    }
    public void PutInHand(GameObject obj)
    {
        inHand = obj;
    }
    
    //---------------------------------------------------
}
