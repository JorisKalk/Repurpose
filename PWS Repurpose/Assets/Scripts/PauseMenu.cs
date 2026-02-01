using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    [SerializeField] GameObject pausedMenu;

    public void Pause ()
    {
        pausedMenu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void Resume ()
    {
        pausedMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void Restart()
    {
        pausedMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
        PlayerLife.lastCheckPointPosition = new Vector2(-6f, 0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Home()
    {
        Time.timeScale = 1;
        isPaused = false;
        SceneManager.LoadScene(0);
    }
}