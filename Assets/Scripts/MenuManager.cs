using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    private static MenuManager _instance = null;

    public GameObject menuCamera;
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject aboutPage;
    public GameObject controlsPage;

    public bool inSubmenu = false;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        aboutPage.SetActive(false);
        controlsPage.SetActive(false);
        
        inSubmenu = false;
        menuCamera.SetActive(true);;
    }

    public void GameStateUpdated(GameManager.GameState state)
    {
        if (state == GameManager.GameState.PREGAME)
        {
            mainMenu.SetActive(true);
            pauseMenu.SetActive(false);
        }
        else if (state == GameManager.GameState.RUNNING)
        {
            mainMenu.SetActive(false);
            pauseMenu.SetActive(false);
        }
        else
        {
            mainMenu.SetActive(false);
            pauseMenu.SetActive(true);
        }
    }

    public void StartButtonPressed()
    {
        Debug.Log("Start pressed");
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        menuCamera.SetActive(false);
        GameManager.Instance.StartGame();
    }

    public void ControlsButtonPressed()
    {
        Debug.Log("Controls pressed");
        inSubmenu = true;
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        controlsPage.SetActive(true);
    }

    public void AboutButtonPressed()
    {
        Debug.Log("About pressed");
        inSubmenu = true;
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        controlsPage.SetActive(true);
    }

    public void ExitButtonPressed()
    {
        Debug.Log("Exit pressed");
        GameManager.Instance.QuitGame();
    }

    public void RestartButtonPressed()
    {
        Debug.Log("Restart pressed");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inSubmenu)
            {
                if (GameManager.Instance.CurrentGameState == GameManager.GameState.PAUSED)
                {
                    // it can be only about page
                    aboutPage.SetActive(false);
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
                    pauseMenu.SetActive(true);
                    GameObject.Find("MainCamera").SetActive(false);
                    menuCamera.SetActive(true);
                }
                else if (GameManager.Instance.CurrentGameState == GameManager.GameState.PAUSED)
                {
                    GameManager.Instance.UpdateState(GameManager.GameState.RUNNING);
                    pauseMenu.SetActive(false);
                    menuCamera.SetActive(false);
                    GameObject.Find("MainCamera").SetActive(true);
                }
            }
        }
    }
}
