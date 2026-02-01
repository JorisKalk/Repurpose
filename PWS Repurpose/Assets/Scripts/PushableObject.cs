using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerMovement movement;
    private PlayerStates states;
    private PlayerLife life;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        states = GameObject.Find("Player").GetComponent<PlayerStates>();
        life = GameObject.Find("Player").GetComponent<PlayerLife>();
    }

    private void Update()
    {
        if (rb.velocity.x > .5f || rb.velocity.x < -.5f)
        {
            states.isItPushing = "yes";
        }
        else if ((rb.velocity.x < .4f && rb.velocity.x > .1f) || (rb.velocity.x < -.1f && rb.velocity.x > -.4f))
        {
            states.isItPushing = "no";
        }

        if (rb.velocity.y < -0.01f)
        {
            life.deadlyFall = true;
        }
        else
        {
            life.deadlyFall = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (movement.property != "pusher")
            {
                MakeStatic();
            }
            else if (movement.property == "pusher")
            {
                MakeDynamic();
            }
        }
    }

    private void MakeDynamic()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
    
    private void MakeStatic()
    {
        rb.bodyType = RigidbodyType2D.Static;
    }
}
