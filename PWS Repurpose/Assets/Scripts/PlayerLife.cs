using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private PlayerMovement species;
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [SerializeField] private LayerMask crush;

    public bool deadlyFall = false;

    //Deze variabele moet public zijn zodat het checkpoint script het kan aanpassen.
    public static Vector2 lastCheckPointPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        species = GetComponent<PlayerMovement>();
        coll = GetComponent<BoxCollider2D>();
        GameObject.FindGameObjectWithTag("Player").transform.position = lastCheckPointPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("deathZone"))
        {
            Die();
        }

        if (collision.gameObject.CompareTag("water") && species.property != "swimmer")
        {
            Die();
        }

        if (collision.gameObject.CompareTag("pushableBox") && IsItCrushed() && deadlyFall)
        {
            Die();
        }
    }

    private bool IsItCrushed()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.up, .1f, crush);
    }

    private void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        RestartLevel();
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
