using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class GameManager : Singleton<GameManager>
{

    private static GameManager instance = null;    
    
    private string _currentLevelName = string.Empty;
    
    private List<AsyncOperation> _loadOperations;


    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED
    }

    private GameState _currentGameState = GameState.PREGAME;

    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        private set { _currentGameState = value; }
    }
    

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        _loadOperations = new List<AsyncOperation>();
        
        MenuManager.Instance.GameStateUpdated(_currentGameState);
    }


    public void UpdateState(GameState state)
    {
        GameState previousState = _currentGameState;
        _currentGameState = state;

        switch (_currentGameState)
        {
            case GameState.PREGAME:
                Time.timeScale = 1.0f;
                break;
            
            case GameState.RUNNING:
                Time.timeScale = 1.0f;
                break;
            
            case GameState.PAUSED:
                Time.timeScale = 0.0f;
                break;
            
            default:
                break;
        }
        
        // dispatch messages
        //OnGameStateChange?.Invoke(_currentGameState, previousState);
        
    }
    
    public void StartGame()
    {
        LoadLevel("Level1");
    }

    public void LoadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        if (ao == null)
        {
            Debug.Log("[Game Manager] unable to load level: " + levelName);
            
            return;
        }
        
        ao.completed += OnLoadOperationComplete;
        _loadOperations.Add(ao);
        
        _currentLevelName = levelName;
    }

    private void OnLoadOperationComplete(AsyncOperation ao)
    {
        if (_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);

            if (_loadOperations.Count == 0)
            {
                UpdateState(GameState.RUNNING);
            }
        }
        
        Debug.Log("Load Complete.");
    }

    public void UnloadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        if (ao == null)
        {
            Debug.Log("[Game Manager] unable to unload level: " + levelName);
            
            return;
        }

        ao.completed += OnUnloadOperationComplete;
    }

    private void OnUnloadOperationComplete(AsyncOperation ao)
    {
        Debug.Log("Unload Complete.");
    }


    public void QuitGame()
    {
        // auto saving, clean up, features for quitting
        Application.Quit();
        Debug.Log("Quit");
    }
    
    public void RestartGame()
    {
        UpdateState(GameState.PREGAME);
    }
}