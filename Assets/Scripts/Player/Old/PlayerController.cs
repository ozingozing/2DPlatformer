using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour
{
    //움직임
    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private bool canMove;
    private bool canFlip;
	private bool isWalking;
    private float movementInputDirection;
    public float movementSpeed = 10.0f;
    private float turnTimer;
    public float turnTimerSet = 0.1f;
	private int facingDirection = 1;

    //피격
    private bool knockback;
    private float knockbackStartTime;
    [SerializeField]
    private float knockbackDuration; //duration 지속
    [SerializeField]
    private Vector2 knockbackSpeed;


	//대쉬
	private bool isDashing;
    public float dashTime = 0.2f;
    public float dashSpeed = 30;
    public float distanceBetweenImages = 0.1f;
    public float dashCoolDown = 2.5f;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash;

	//점프
	public Transform groundCheck;
    public LayerMask whatIsGround;
    private float jumpTimer;
    public float jumpTimerSet = 0.15f;
    public float jumpForce = 16.0f;
    public float groundCheckRadius;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableMultiplier = 0.5f;
    public float variableJumpHeightMultiplier = 0.5f;
    public bool isGrounded;
    private bool canNormalJump;
    private bool canWallJump;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    public int amountOfJumps = 1;
    private int amountOfJumpsLest;


    //모서리 오르기
    public Transform ledgeCheck;
    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
	private Vector2 ledgePos2;
    public float ledgeClimbXoffset1, ledgeClimbYoffset1 = 0f;
	public float ledgeClimbXoffset2, ledgeClimbYoffset2 = 0f;
	private bool isTouchingLedge;
    private bool canClimbLedge;
    private bool ledgeDetected;

    //벽 오르기
    public Transform wallCheck;
    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;
    public bool isTouchingWall;
    private bool isWallSliding;
    private bool hasWallJumped;
    public float wallCheckDistance;
    public float wallSlidingSpeed;
    public float wallHopForce;
    public float wallJumpForce;
    private float wallJumpTimer;
    public float wallJumpTimerSet = 0.5f;
    private int lastWallJumpDirection;
    

    private Animator anim;
    

	private void Awake()
	{
		//움직임
		movementSpeed = 10.0f;

		//점프
		jumpForce = 16.0f;
		airDragMultiplier = 0.95f;
		variableMultiplier = 0.5f;
		variableJumpHeightMultiplier = 0.5f;
		amountOfJumps = 1;
	}


	void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLest = amountOfJumps;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }

    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckJump();
        CheckLedgeClimb();
        CheckDash();
        CheckKnockback();
	}

	private void FixedUpdate()
	{
		ApplyMovement();
		CheckSurroundings();
	}


	private void CheckInput()
	{
		movementInputDirection = Input.GetAxisRaw("Horizontal");

		if (Input.GetButton("Jump"))
		{
			if (isGrounded || (amountOfJumpsLest > 0 && isTouchingWall))
			{
				NormalJump();
			}
			else
			{
				jumpTimer = jumpTimerSet;
				isAttemptingToJump = true;
			}
		}

		if (Input.GetButtonDown("Horizontal") && isTouchingWall)
		{
			if (!isGrounded && movementInputDirection != facingDirection)
			{
				canFlip = false;
				canMove = false;

				turnTimer = turnTimerSet;
			}
		}

		if (turnTimer >= 0)
		{
			turnTimer -= Time.deltaTime;

			if (turnTimer <= 0)
			{
				canMove = true;
				canFlip = true;
			}
		}

		if (checkJumpMultiplier && !Input.GetButton("Jump"))
		{
			checkJumpMultiplier = false;
			rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableMultiplier);
		}

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("Dash");
            if(Time.time >= (lastDash + dashCoolDown))
            {
				AttemptToDash();
			}
        }
	}

	private void CheckDash()
    {
        if(isDashing)
        {
            if(dashTimeLeft > 0)
            {
				canMove = false;
				canFlip = false;
				rb.velocity = new Vector2(dashSpeed * facingDirection, 0);
				dashTimeLeft -= Time.deltaTime;

				if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
				{
                    Debug.Log("대쉬생성");
					PlayerAfterImagePool.Instance.GetFromPool();
					lastImageXpos = transform.position.x;
				}
			}
            
            if(dashTimeLeft <= 0 || isTouchingWall)
            {
                isDashing = false;
                canMove = true;
                canFlip = true;
            }
        }
    }

	private void CheckIfWallSliding()
    {
        if(isTouchingWall
            && movementInputDirection == facingDirection && rb.velocity.y < 0
            && !canClimbLedge)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    public bool GetDashStatus()
    {
        return isDashing;
    }

    private void CheckIfCanJump()
    {
        if(isGrounded && rb.velocity.y <= 0.01f)
        {
            amountOfJumpsLest = amountOfJumps;
        }

        if(isTouchingWall)
        {
            canWallJump = true;
        }

        if(amountOfJumpsLest <= 0)
        {
            canNormalJump = false;
        }    
        else
        {
            canNormalJump = true;
        }    
    }

    

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius,
            whatIsGround);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance,
            whatIsGround);

        isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, wallCheckDistance,
            whatIsGround);

        if(isTouchingWall && !isTouchingLedge && !ledgeDetected)
        {
            ledgeDetected = true;
            ledgePosBot = wallCheck.position;
        }
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

        if(Mathf.Abs(rb.velocity.x) >= 0.01f)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    public int GetFacingDirection()
    {
        return facingDirection;
    }

	private void CheckJump()
	{
		if (jumpTimer > 0)
		{
			//WallJump
			if (!isGrounded && isTouchingWall && movementInputDirection != 0 && movementInputDirection != facingDirection)
			{
				WallJump();
			}
			else if (isGrounded)
			{
				NormalJump();
			}
		}

		if (isAttemptingToJump)
		{
			jumpTimer -= Time.deltaTime;
		}

		if (wallJumpTimer > 0)
		{
			if (hasWallJumped && movementInputDirection == -lastWallJumpDirection)
			{
				rb.velocity = new Vector2(rb.velocity.x, 0.0f);
				hasWallJumped = false;
			}
			else if (wallJumpTimer <= 0)
			{
				hasWallJumped = false;
			}
			else
			{
				wallJumpTimer -= Time.deltaTime;
			}
		}
	}

    public void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }


    private void CheckKnockback()
    {
        if((Time.time > knockbackStartTime + knockbackDuration) && knockback)
        {
            knockback = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void CheckLedgeClimb()
    {
        if(ledgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;

            //ledgePos1 출발지점 ledgePos2 도착지점
            if (isFacingRight)
            {
                ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) - ledgeClimbXoffset1,
                    Mathf.Floor(ledgePosBot.y) + ledgeClimbYoffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) + ledgeClimbXoffset2,
                    Mathf.Floor(ledgePosBot.y) + ledgeClimbYoffset2);
            }
            else //반대 방향
            {
                ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) + ledgeClimbXoffset1,
                    Mathf.Floor(ledgePosBot.y) + ledgeClimbYoffset1);
                ledgePos2 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) - ledgeClimbXoffset2,
                    Mathf.Floor(ledgePosBot.y) + ledgeClimbYoffset2);
            }

            canMove = false;
            canFlip = false;

            anim.SetBool("canClimbLedge", canClimbLedge);
		}

        if(canClimbLedge)
        {
            transform.position = ledgePos1;
        }
    }

	private void AttemptToDash()
	{
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
	}


	private void UpdateAnimations()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
	}
    
    public void FinishLedgeClimb()
    {
        canClimbLedge = false;
		transform.position = ledgePos2;
		canMove = true;
        canFlip = true;
        ledgeDetected = false;
		anim.SetBool("canClimbLedge", canClimbLedge);
	}

    private void NormalJump()
    {
		if (canNormalJump)
		{
			Debug.Log("그냥 점프");
			rb.velocity = new Vector2(rb.velocity.x, jumpForce);
			amountOfJumpsLest--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
		}
	}

    private void WallJump()
    {
        if (canWallJump)
		{
			Debug.Log("반대로 점프!");
            rb.velocity = new Vector2 (rb.velocity.x, 0.0f);
			isWallSliding = false;
            amountOfJumpsLest = amountOfJumps;
			amountOfJumpsLest--;
			Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
			rb.AddForce(forceToAdd, ForceMode2D.Impulse);
			jumpTimer = 0;
			isAttemptingToJump = false;
			checkJumpMultiplier = true;
            turnTimer = 0;
            canMove = true;
            canFlip = true;
            hasWallJumped = true;
            wallJumpTimer = wallJumpTimerSet;
            lastWallJumpDirection = -facingDirection;
		}
	}

    private void ApplyMovement()
    {
		if (!isGrounded && !isWallSliding && movementInputDirection == 0
            && !knockback) // 점프한 상태에서 방향키 안 눌렀을 때
		{
			rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
		}
		else if(canMove && !knockback) // 점프하고 방향키도 눌렀을 때
		{
			rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
		}

		if (isWallSliding) //슬라이딩 중
        {
            if(rb.velocity.y < -wallSlidingSpeed) //내려가고 있으면
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed); //계속 내려보냄
            }
        }
        
    }

    public void DisableFlip()
    {
        canFlip = false;
    }

    public void EnableFlip()
    {
        canFlip = true;
    }

	private void Flip()
    {
        if(!isWallSliding && canFlip && !knockback)
        {
            facingDirection *= -1;
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
