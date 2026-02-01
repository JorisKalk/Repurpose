using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonPress : MonoBehaviour
{
    [SerializeField] private int level = 1;

    public void OnButtonPress()
    {
        Time.timeScale = 1;
        PauseMenu.isPaused = false;
        PlayerLife.lastCheckPointPosition = new Vector2(-6f, 0f);
        SceneManager.LoadScene(level);
    }
}
