using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool GamePaused = false;

    public GameObject pauseMenuUI;
    public GameObject optionMenuUI;
    public GameObject backgroundUI;

    void Start()
    {
        Time.timeScale = 1f;
        GamePaused = false ;
    }

    void Update()
    {
        if (Input.GetButtonDown("MenuButton"))
        {
            if (GamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        backgroundUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }
    
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        optionMenuUI.SetActive(false);
        backgroundUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false ;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
