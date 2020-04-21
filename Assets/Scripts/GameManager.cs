using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class GameManager : Singleton<GameManager>
{

    public float TimeLeft = 300f;
    public int WarningsLeft = 3;
    
    private static GameManager instance = null;    
    
    private string _currentLevelName = string.Empty;
    
    private List<AsyncOperation> _loadOperations;
    
    public static int ComputersBrokeCount;
    public int BrokeCount => ComputersBrokeCount;
    

    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED
    }

    private GameState _currentGameState = GameState.PREGAME;
    
    private float _originalTime;
    private int _originalWarnings;
    private bool _levelLoaded;


    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        private set { _currentGameState = value; }
    }
    

    private void Start()
    {
        _levelLoaded = false;
        _originalTime = TimeLeft;
        _originalWarnings = WarningsLeft;
        
        DontDestroyOnLoad(gameObject);
        
        _loadOperations = new List<AsyncOperation>();
        
        MenuManager.Instance.UpdateWarnings();
        MenuManager.Instance.GameStateUpdated(_currentGameState);
    }
    
    public Action<Computer.Owner> ComputerBroke = ComputerBroked;

    private static void ComputerBroked(Computer.Owner owner)
    {
        ComputersBrokeCount++;

        if (owner == Computer.Owner.Delegator)
        {
            Debug.Log("broken by deleg");
            MenuManager.Instance.UpdatePlayerSpeakBox(TextManager.Instance.GetDelegatorProblem());
            MenuManager.Instance.UpdateEnemySpeakBox(TextManager.Instance.GetDelegatorText(),
                EnemyController.EnemyType.Delegator);

        }

        if (owner == Computer.Owner.Manager)
        {
            Debug.Log("broken by man");
            MenuManager.Instance.UpdatePlayerSpeakBox(TextManager.Instance.GetBossProblem());
            MenuManager.Instance.UpdateEnemySpeakBox(TextManager.Instance.GetBossText(),
                EnemyController.EnemyType.Manager);
        }
        
        Debug.Log("broken: " + ComputersBrokeCount.ToString());
    }

    public Action<Computer.Owner> ComputerFix = ComputerFixed;

    private static void ComputerFixed(Computer.Owner owner)
    {
        ComputersBrokeCount--;
        
        Debug.Log("fixed: " + ComputersBrokeCount.ToString());
        
        MenuManager.Instance.UpdatePlayerSpeakBox(TextManager.Instance.GetComputerFixedString());

        if (owner != Computer.Owner.None)
        {
            MenuManager.Instance.UpdateEnemySpeakBox(TextManager.Instance.GetComputerFixedEnemyString(), 
                owner == Computer.Owner.Delegator ? EnemyController.EnemyType.Delegator : EnemyController.EnemyType.Manager);
        }
        
        if (ComputersBrokeCount <= 0)
        {
            Debug.Log("Game won");
            Instance.GameOver(true);
        }
    }

    public void PlayerCaughtByManager()
    {
        Instance.WarningsLeft--;
        MenuManager.Instance.UpdateWarnings();
        AudioManager.Instance.Play("broken");
        
        if (Instance.WarningsLeft <= 0)
        {
            Instance.GameOver(false);
        }
    }

    public void PlayerCaughtByDelegator(float penalty)
    {
        TimeLeft -= penalty;
    }


    public void UpdateState(GameState state)
    {
        GameState previousState = _currentGameState;
        _currentGameState = state;

        switch (_currentGameState)
        {
            case GameState.PREGAME:
                Time.timeScale = 0.0f;
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
        
        //MenuManager.Instance.GameStateUpdated(state);
        
    }

    private static void ResetFix()
    {
        ComputersBrokeCount = 0;
    }
    
    public void StartGame()
    {
        TimeLeft = _originalTime;
        WarningsLeft = _originalWarnings;
        ResetFix();
        LoadLevel("Level1");
    }

    public void LoadLevel(string levelName)
    {
        if (_levelLoaded)
        {
            UnloadLevel(_currentLevelName);
        }
        
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
                _levelLoaded = true;
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
        UnloadLevel(_currentLevelName);
        UpdateState(GameState.PREGAME);
        StartGame();
    }

    public void GameOver(bool won)
    {
        // TODO: implement
        MenuManager.Instance.GameOver();
        UpdateState(GameState.RUNNING);
        if (won)
        {
            MenuManager.Instance.UpdateEnemySpeakBox("Yesterday you outdid yourself! Trust me, I do it everyday myself!", EnemyController.EnemyType.Manager);
            MenuManager.Instance.UpdatePlayerSpeakBox("Here we go again..");
        }
        else
        {
            MenuManager.Instance.UpdateEnemySpeakBox("You are getting Outsourced! GET OUT!", EnemyController.EnemyType.Manager);
            MenuManager.Instance.UpdatePlayerSpeakBox("I guess its Game Over.. But WAIT! Time travel?!");
        }
        Instance.RestartGame();
        Debug.Log("GameOver");
    }
}