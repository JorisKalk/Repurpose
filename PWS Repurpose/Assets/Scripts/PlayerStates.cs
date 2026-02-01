using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStates : MonoBehaviour
{
    [SerializeField] private LayerMask groundedGround;

    public string isItPushing = "no";
    public string attacking = "not";
    
    private SpriteRenderer sprite;
    private PlayerMovement movement;
    private BoxCollider2D coll;
    private int level;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        movement = GetComponent<PlayerMovement>();
        coll = GetComponent<BoxCollider2D>();
        level = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && Switchable())
        {
            movement.property = "default";
            sprite.color = new Color(1f, 1f, 1f, 1f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && Switchable() && level >= 4f)
        {
            movement.property = "bouncer";
            sprite.color = new Color(0f, 1f, 0.1104f, 1f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && Switchable() && level >= 5f)
        {
            movement.property = "dasher";
            sprite.color = new Color(0f, 1f, 0.9806142f, 1f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && Switchable() && level >= 6f)
        {
            movement.property = "swimmer";
            sprite.color = new Color(0f, 0.01862764f, 1f, 1f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && Switchable() && level >= 7f)
        {
            movement.property = "walljumper";
            sprite.color = new Color(0.009344961f, 0.3962264f, 0.09022435f, 1f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6) && Switchable() && level >= 8f)
        {
            movement.property = "pusher";
            sprite.color = new Color(1f, 0.9862055f, 0f, 1f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7) && Switchable() && level >= 9f)
        {
            movement.property = "destroyer";
            sprite.color = new Color(1f, 0f, 0f, 1f);
        }
    }

    private bool Switchable()
    {
        if (OnGround() && NotDashing() && PushingOrNot() && AttackingOrNot() && !IsGamePaused())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    private bool OnGround()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, groundedGround);
    }

    private bool NotDashing()
    {
        return movement.dashState == "false";
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("pushableBox"))
        {
            isItPushing = "no";
        }
    }

    private bool PushingOrNot()
    {
        if (isItPushing == "no")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool AttackingOrNot()
    {
        if (attacking == "not")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsGamePaused()
    {
        if (PauseMenu.isPaused == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
