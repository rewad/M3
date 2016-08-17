using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using System;
using System.Collections;

public enum EGameState
{
    EPauseState = 0,
    EGameOverState,
    EPlayState,
    EExitState,
}




public class GameInstance : MonoBehaviour
{ 
    private Grid m_grid;  
    private float m_timeGame;
    private int m_scoreGame; 
    private EGameState m_currentState;
    private static GameInstance m_instance; 
    private GameUI m_gameUI;
    private int m_difficulty;
    void Awake()
    {

        Advertisement.Initialize("1099159");
        if (m_instance == null)
        {
            m_instance = FindObjectOfType<GameInstance>();
        }

        m_gameUI = FindObjectOfType<GameUI>();
        m_gameUI.CloseMenu();
    }
    void Update()
    {
        if (m_currentState == EGameState.EPlayState)
        {
            m_timeGame += Time.deltaTime;
            UpdateUI();
        }
    }

    public static GameInstance Get()
    {
        if (m_instance == null)
        {
            m_instance = FindObjectOfType<GameInstance>();
        }
        return m_instance;
    }
    public void StartNewGame()
    {
        StartNewGame(m_difficulty);
    }
    public void StartNewGame(int difficulty)
    {
        m_difficulty = difficulty;
        if (m_grid == null)
        {
            m_grid = gameObject.GetComponent<Grid>();

            if (m_grid == null)
                m_grid = gameObject.AddComponent<Grid>();
        }

        m_grid.Clear();
        StartCoroutine(m_grid.GeneratorRandomGrid(m_difficulty));
    }
    public void SetState(EGameState state)
    {
        m_currentState = state;
        switch (state)
        {
            case EGameState.EPauseState:
                Time.timeScale = 0.0f; 
                break;
            case EGameState.EPlayState:
                Time.timeScale = 1.0f;
                break;
            case EGameState.EExitState: 
                m_grid.Clear();
                break;
        }
    } 
    private void UpdateUI()
    {
        m_gameUI.UpdateScoreUI(m_scoreGame, m_timeGame);
    }  
    public void UpdateScore(int score)
    {
        m_scoreGame += score;
    }   
}
