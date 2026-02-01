using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Finish : MonoBehaviour
{
    [SerializeField] GameObject levelEndMenu;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CompleteLevel();
    }

    public void CompleteLevel()
    {
        levelEndMenu.SetActive(true);
        GameObject.Find("Player").GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GameObject.Find("Player").SetActive(false);
    }
}
