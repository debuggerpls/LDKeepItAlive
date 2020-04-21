using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : Singleton<MenuManager>
{
    private static MenuManager _instance = null;

    public GameObject menuCamera;
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject aboutPage;
    public GameObject controlsPage;

    public GameObject backgroundImage;

    public GameObject statsGui;
    public GameObject itemsText;
    
    public GameObject itemGuiHolder;
    public GameObject itemGuiPrefab;

    public Text timeText;
    public Text fixText;
    public Text warningText;

    public Text playerText;
    public Text enemyText;
    public Image enemyImage;
    public Sprite enemyDelegatorHead;
    public Sprite enemyManagerHead;
    public Sprite enemyHottieHead;
    
    
    public bool inSubmenu = false;

    private GameManager.GameState _myState;
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        _myState = GameManager.GameState.PREGAME;
        
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        aboutPage.SetActive(false);
        controlsPage.SetActive(false);
        backgroundImage.SetActive(false);
        itemsText.SetActive(false);
        statsGui.SetActive(false);
        itemGuiHolder.SetActive(false);
        
        inSubmenu = false;
        menuCamera.SetActive(true);;
    }

    public void GameStateUpdated(GameManager.GameState state)
    {
        if (state == GameManager.GameState.PREGAME)
        {
            itemGuiHolder.SetActive(false);
            itemsText.SetActive(false);
            statsGui.SetActive(false);
            backgroundImage.SetActive(true);
            mainMenu.SetActive(true);
            pauseMenu.SetActive(false);
        }
        else if (state == GameManager.GameState.RUNNING)
        {
            itemGuiHolder.SetActive(true);
            itemsText.SetActive(true);
            statsGui.SetActive(true);
            backgroundImage.SetActive(false);
            mainMenu.SetActive(false);
            pauseMenu.SetActive(false);
        }
        else
        {
            itemGuiHolder.SetActive(false);
            itemsText.SetActive(false);
            statsGui.SetActive(false);
            backgroundImage.SetActive(true);
            mainMenu.SetActive(false);
            pauseMenu.SetActive(true);
        }

        _myState = state;
    }

    public void GameOver()
    {
        foreach (var item in itemGuiHolder.GetComponentsInChildren<ItemGui>())
        {
            Destroy(item.gameObject);
        }
    }

    public void StartButtonPressed()
    {
        AudioManager.Instance.Play("select");
        Debug.Log("Start pressed");
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        menuCamera.SetActive(false);
        backgroundImage.SetActive(false);
        itemGuiHolder.SetActive(true);
        itemsText.SetActive(true);
        statsGui.SetActive(true);
        foreach (var item in itemGuiHolder.GetComponentsInChildren<ItemGui>())
        {
            Destroy(item.gameObject);
        }
        GameManager.Instance.StartGame();
    }

    public void ControlsButtonPressed()
    {
        AudioManager.Instance.Play("select");
        Debug.Log("Controls pressed");
        inSubmenu = true;
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        controlsPage.SetActive(true);
    }

    public void AboutButtonPressed()
    {
        AudioManager.Instance.Play("select");
        Debug.Log("About pressed");
        inSubmenu = true;
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        aboutPage.SetActive(true);
    }

    public void ExitButtonPressed()
    {
        AudioManager.Instance.Play("select");
        Debug.Log("Exit pressed");
        GameManager.Instance.QuitGame();
    }

    public void RestartButtonPressed()
    {
        AudioManager.Instance.Play("select");
        Debug.Log("Restart pressed");
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        menuCamera.SetActive(false);
        backgroundImage.SetActive(false);


        foreach (var item in itemGuiHolder.GetComponentsInChildren<ItemGui>())
        {
            Destroy(item.gameObject);
        }
        
        GameManager.Instance.RestartGame();
    }

    public void UpdateWarnings()
    {
        warningText.text = GameManager.Instance.WarningsLeft.ToString();
    }

    public void UpdatePlayerSpeakBox(String newText)
    {
        playerText.text = newText;
    }

    public void UpdateEnemySpeakBox(String newText, EnemyController.EnemyType type)
    {
        enemyText.text = newText;

        enemyImage.enabled = true;
        switch (type)
        {
            case EnemyController.EnemyType.Delegator:
                enemyImage.sprite = enemyDelegatorHead;
                break;
            case EnemyController.EnemyType.Hottie:
                enemyImage.sprite = enemyHottieHead;
                break;
            case EnemyController.EnemyType.Manager:
                enemyImage.sprite = enemyManagerHead;
                break;
        }
    }
    
    public void AddItemToGui(Sprite sprite, ItemGui.ItemGuiType type, int keyNr, Color color)
    {
        var newItem = Instantiate(itemGuiPrefab, itemGuiHolder.transform);
        var itemGui = newItem.GetComponent<ItemGui>();
        itemGui.SetItem(sprite, type, keyNr, color);
    }
    
    public void RemoveItemFromGui(ItemGui.ItemGuiType type, int keyNr)
    {
        foreach (var item in itemGuiHolder.GetComponentsInChildren<ItemGui>())
        {
            if (item.RemoveItem(type, keyNr))
            {
                return;
            }
        }
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.PAUSED ||
            GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING)
        {
            var ts = TimeSpan.FromSeconds(GameManager.Instance.TimeLeft);
            timeText.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
            fixText.text = GameManager.Instance.BrokeCount.ToString();
        }
        

    }

    private void Update()
    {
        if (_myState != GameManager.Instance.CurrentGameState)
        {
            GameStateUpdated(GameManager.Instance.CurrentGameState);
        }
        
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING)
        {
            GameManager.Instance.TimeLeft -= Time.deltaTime;
            if (GameManager.Instance.TimeLeft <= 0)
            {
                GameManager.Instance.GameOver(false);
            }
        }
        
        
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inSubmenu)
            {
                if (GameManager.Instance.CurrentGameState == GameManager.GameState.PAUSED)
                {
                    // it can be only about page
                    aboutPage.SetActive(false);
                    controlsPage.SetActive(false);
                    pauseMenu.SetActive(true);
                }
                else
                {
                    // in pregame so turn both off
                    aboutPage.SetActive(false);
                    controlsPage.SetActive(false);
                    mainMenu.SetActive(true);
                }
            }
            else
            {
                if (GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING)
                {
                    GameManager.Instance.UpdateState(GameManager.GameState.PAUSED);
                    backgroundImage.SetActive(true);
                    pauseMenu.SetActive(true);
                    GameObject.Find("MainCamera").GetComponent<Camera>().enabled = false;
                    menuCamera.SetActive(true);
                }
                else if (GameManager.Instance.CurrentGameState == GameManager.GameState.PAUSED)
                {
                    GameManager.Instance.UpdateState(GameManager.GameState.RUNNING);
                    pauseMenu.SetActive(false);
                    menuCamera.SetActive(false);
                    backgroundImage.SetActive(false);
                    GameObject.Find("MainCamera").GetComponent<Camera>().enabled = true;
                }
            }
        }
    }
}
