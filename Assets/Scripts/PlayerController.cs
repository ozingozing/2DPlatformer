using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private float movementInputDirection;
    public float movementSpeed = 10.0f;

    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float jumpForce = 16.0f;
    public float groundCheckRadius;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableMultiplier = 0.5f;
    public bool isGrounded;
    private bool canJump;
    public int amountJumps = 1;
    private int amountOfJumpsLest;
    


    public Transform wallCheck;
    public bool isTouchingWall;
    private bool isWallSliding;
    public float wallCheckDistance;
    public float wallSlidingSpeed;
    

    private Animator anim;
    private bool isWalking;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLest = amountJumps;
    }

    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckIfWallSliding();
    }

    private void CheckIfWallSliding()
    {
        if(isTouchingWall && !isGrounded && rb.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void CheckIfCanJump()
    {
        if(isGrounded && rb.velocity.y <= 0)
        {
            amountOfJumpsLest = amountJumps;
        }

        if(amountOfJumpsLest <= 0)
        {
            canJump = false;
        }    
        else
        {
            canJump = true;
        }    
    }

    private void FixedUpdate()
	{
        ApplyMovement();
        CheckSurroundings();
	}

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius,
            whatIsGround);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance,
            whatIsGround);
    }

    private void CheckMovementDirection()
    {
        if(isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if(!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        if(rb.velocity.x != 0
            && (Input.GetKey(KeyCode.A)
            || Input.GetKey(KeyCode.D)))
        {
            Debug.Log(rb.velocity.x);
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
    }
    

	private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if(Input.GetButtonUp("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableMultiplier);
        }
    }

	private void Jump()
	{
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLest--;
        }
	}

    private void ApplyMovement()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }
        else if(!isGrounded && !isWallSliding && movementInputDirection != 0)
        {
            Vector2 forceToAdd = new Vector2(movementForceInAir * movementInputDirection, 0);
            rb.AddForce(forceToAdd);

            if(Mathf.Abs(rb.velocity.x) > movementSpeed)
            {
                rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
            }
        }
        else if (!isGrounded && !isWallSliding && movementInputDirection == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }


        if (isWallSliding)
        {
            if(rb.velocity.y < -wallSlidingSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
            }
        }
        
    }

	private void Flip()
    {
        if(!isWallSliding)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance,
            wallCheck.position.y, wallCheck.position.z));
    }
}
