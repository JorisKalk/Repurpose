using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBox : MonoBehaviour
{
    private PlayerMovement movement;
    private SpriteRenderer playerSprite;
    private PlayerStates species;
    private BoxCollider2D left;
    private BoxCollider2D right;

    [SerializeField] private LayerMask player;
    
    private void Start()
    {
        movement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerSprite = GameObject.Find("Player").GetComponent<SpriteRenderer>();
        species = GameObject.Find("Player").GetComponent<PlayerStates>();
        left = transform.GetChild(0).GetComponent<BoxCollider2D>();
        right = transform.GetChild(1).GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (Hit() && movement.destroyNow)
        {
            Destroy(gameObject);
        }
    }

    private bool Hit()
    {
        if (species.attacking == "itIs" && playerSprite.flipX == false)
        {
            return Physics2D.BoxCast(left.bounds.center, left.bounds.size, 0f, Vector2.zero, .1f, player);
        }
        else if (species.attacking == "itIs" && playerSprite.flipX == true)
        {
            return Physics2D.BoxCast(right.bounds.center, right.bounds.size, 0f, Vector2.zero, .1f, player);
        }
        else
        {
            return false;
        }
    }
}
