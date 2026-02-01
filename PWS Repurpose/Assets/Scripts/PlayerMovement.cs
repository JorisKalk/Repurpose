using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    private PlayerStates species;

    //Deze is belangrijk om te zorgen dat het springen juist blijft werken. Deze staat dus apart, omdat er geen veranderingen aan gemaakt moeten worden.
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask jumpableWater;
    [SerializeField] private LayerMask jumpableUnderwater;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float bounceForce = 20f;
    [SerializeField] private float dashSpeed = 50f;
    [SerializeField] private float wallJumpTime = .2f;
    [SerializeField] private float attackTime = .7f;

    public bool animateOrNot = true;

    //Deze is public zodat het PlayerStates script het kan aanpassen.
    public string property = "default";

    private enum MovementState { idle, running, jumping, falling }
    public string dashState = "false";

    //Walljumper variables.
    private bool isGrabbing;
    private float movementCounter;
    private string lastWallJumpSide = "none";

    //Destroyer variables.
    public bool destroyNow;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        species = GetComponent<PlayerStates>();
    }

    //Bovenste is rennen en de onderste is springen.
    private void Update()
    {
        //Deze if is belangrijk om te zorgen dat de walljump gebruikersvriendelijk is.
        if (movementCounter <= 0f && PauseMenu.isPaused == false)
        {
            dirX = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

            if (Input.GetButtonDown("Jump") && (IsGrounded() || IsGroundedUnderwater()) && property == "bouncer")
            {
                rb.velocity = new Vector2(rb.velocity.x, bounceForce);
            }
            else if (Input.GetButtonDown("Jump") && (IsGrounded() || IsGroundedInWater() || IsGroundedUnderwater()))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }


            //Dasher
            if (Input.GetKeyDown(KeyCode.Q) && property == "dasher" && dashState == "false")
            {
                dashState = "dashLeft";
            }
            else if (Input.GetKeyDown(KeyCode.E) && property == "dasher" && dashState == "false")
            {
                dashState = "dashRight";
            }

            if (dashState == "dashLeft")
            {
                rb.velocity = new Vector2(dashSpeed * -1f, 0f);
                rb.gravityScale = 0f;
            }
            else if (dashState == "dashRight")
            {
                rb.velocity = new Vector2(dashSpeed, 0f);
                rb.gravityScale = 0f;
            }

            if (HitWallLeft() && property == "dasher" && dashState == "dashLeft")
            {
                dashState = "false";
                rb.gravityScale = 3f;
            }
            else if (HitWallRight() && property == "dasher" && dashState == "dashRight")
            {
                dashState = "false";
                rb.gravityScale = 3f;
            }

            //Walljumper
            isGrabbing = false;
            if (property == "walljumper")
            {
                if (HitWallLeft() && !IsGrounded() && lastWallJumpSide != "left")
                {
                    if (sprite.flipX == true && dirX < 0f)
                    {
                        isGrabbing = true;
                    }
                }
                else if (HitWallRight() && !IsGrounded() && lastWallJumpSide != "right")
                {
                    if (sprite.flipX == false && dirX > 0f)
                    {
                        isGrabbing = true;
                    }
                }

                if (isGrabbing)
                {
                    rb.gravityScale = 0f;
                    rb.velocity = Vector2.zero;

                    if (Input.GetButtonDown("Jump"))
                    {
                        movementCounter = wallJumpTime;

                        rb.velocity = new Vector2(-dirX * moveSpeed, jumpForce);
                        rb.gravityScale = 3f;
                        isGrabbing = false;

                        if (dirX > 0f)
                        {
                            lastWallJumpSide = "right";
                        }
                        else if (dirX < 0f)
                        {
                            lastWallJumpSide = "left";
                        }

                        if (rb.velocity.x > 0f)
                        {
                            sprite.flipX = false;
                        }
                        else if (rb.velocity.x < 0f)
                        {
                            sprite.flipX = true;
                        }
                    }
                }
                else if (!isGrabbing)
                {
                    rb.gravityScale = 3f;
                }

                if (IsGrounded())
                {
                    lastWallJumpSide = "none";
                }
            }

            //Destroyer
            if (Input.GetKeyDown(KeyCode.E) && property == "destroyer" && IsGrounded())
            {
                anim.SetBool("attack", true);
                species.attacking = "itIs";
                rb.bodyType = RigidbodyType2D.Static;
                movementCounter = attackTime;
            }

        }
        else
        {
            movementCounter -= Time.deltaTime;
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {   
        if (animateOrNot == true)
        {
            MovementState state;
            
            if (dirX > 0f && dashState == "false" && movementCounter <= 0f)
            {
                state = MovementState.running;
                sprite.flipX = false;
            }
            else if (dirX < 0f && dashState == "false" && movementCounter <= 0f)
            {
                state = MovementState.running;
                sprite.flipX = true;
            }
            else
            {
                state = MovementState.idle;
            }

            if (rb.velocity.y > .1f)
            {
                state = MovementState.jumping;
            }
            else if (rb.velocity.y < -.1f)
            {
                state = MovementState.falling;
            }
            
            //Animations voor dasher.
            if (dashState == "dashLeft")
            {
                anim.SetBool("dashing", true);
                sprite.flipX = true;
            }
            else if (dashState == "dashRight")
            {
                anim.SetBool("dashing", true);
                sprite.flipX = false;
            }
            else if (dashState == "false")
            {
                anim.SetBool("dashing", false);
            }
            
            //Animations voor walljumper.
            if (isGrabbing)
            {
                anim.SetBool("isGrabbing", true);
            }
            else if (!isGrabbing)
            {
                anim.SetBool("isGrabbing", false);
            }
            
            anim.SetInteger("state", (int)state);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private bool IsGroundedInWater()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableWater);
    }

    private bool IsGroundedUnderwater()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableUnderwater);
    }

    private bool HitWallLeft()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.left, .1f, jumpableGround);
    }

    private bool HitWallRight()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.right, .1f, jumpableGround);
    }

    //De twee methods hieronder worden uitgevoerd door triggers in de attack animatie.
    private void DestroyItNow()
    {
        destroyNow = true;
    }

    private void AttackEnd()
    {
        species.attacking = "not";
        anim.SetBool("attack", false);
        rb.bodyType = RigidbodyType2D.Dynamic;
        destroyNow = false;
    }
}
