using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndMenu : MonoBehaviour
{
    public void NextLevel()
    {
        PlayerLife.lastCheckPointPosition = new Vector2(-6f, 0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        PlayerLife.lastCheckPointPosition = new Vector2(-6f, 0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void HomeMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LevelMenu()
    {
        SceneManager.LoadScene(1);
    }
}
