using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
	#region State Variables
	public PlayerStateMachine StateMachine { get; private set; }
	public PlayerIdleState IdleState { get; private set; }
	public PlayerMoveState MoveState { get; private set; }
	public PlayerJumpState JumpState { get; private set; }
	public PlayerInAirState InAirState { get; private set; }
	public PlayerLandState LandState { get; private set; }
	public PlayerWallSlideState WallSlideState { get; private set;}
	public PlayerWallGrabState WallGrabState { get; private set;}
	public PlayerWallClimbState WallClimbState { get; private set; }
	public PlayerWallJumpState WallJumpState { get; private set;}
	public PlayerLedgeClimbState LedgeClimbState { get; private set; }
	public PlayerDashState DashState { get; private set; }
	public PlayerCrouchIdleState CrouchIdleState { get; private set; }
	public PlayerCrouchMoveState CrouchMoveState { get; private set; }
	public PlayerAttackState PrimaryAttackState { get; private set; }
	public PlayerAttackState SecondaryAttackState { get; private set; }

	[SerializeField]
	private PlayerData playerData;
	#endregion

	#region Components
	public Animator Anim { get; private set; }
	public PlayerInputHandler InputHandler { get; private set; }
	public Rigidbody2D RB { get; private set; }
	public Transform DashDirectionIndicator { get; private set; }
	public BoxCollider2D MovementCollider { get; private set; }
	public PlayerInventory Inventory { get; private set; }

	#endregion

	#region Check Transforms

	[SerializeField]
	private Transform groundCheck;
	[SerializeField]
	public Transform wallCheck;
	[SerializeField]
	public Transform ledgeCheck;
	[SerializeField]
	private Transform ceilingCheck;
	#endregion

	#region Other Variables
	public Vector2 CurrentVelocity { get; private set; }
	public int FacingDirection { get; private set; }

	private Vector2 workspace;
	#endregion

	#region Unity Callback Functions
	private void Awake()
	{
		StateMachine = new PlayerStateMachine();

		IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
		MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
		JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
		InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
		LandState = new PlayerLandState(this, StateMachine, playerData, "land");
		WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
		WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "wallGrab");
		WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "wallClimb");
		WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");
		LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "ledgeClimbState");
		DashState = new PlayerDashState(this, StateMachine, playerData, "inAir");
		CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, playerData, "crouchIdle");
		CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, playerData, "crouchMove");
		PrimaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
		SecondaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
	}

	private void Start()
	{
		Anim = GetComponent<Animator>();
		InputHandler = GetComponent<PlayerInputHandler>();
		RB = GetComponent<Rigidbody2D>();
		DashDirectionIndicator = transform.GetChild(3);
		MovementCollider = GetComponent<BoxCollider2D>();
		Inventory = GetComponent<PlayerInventory>();

		FacingDirection = 1;

		PrimaryAttackState.SetWeapon(Inventory.weapons[(int)CombatInputs.primary]);
		//SecondaryAttackState.SetWeapon(Inventory.weapons[(int)CombatInputs.secondary]);

		StateMachine.Initialize(IdleState);
	}

	private void Update()
	{
		CurrentVelocity = RB.velocity;
		StateMachine.CurrentState.LogicUpdate();
	}

	private void FixedUpdate()
	{
		StateMachine.CurrentState.PhysicsUpdate();
	}
	#endregion

	#region Set Functions

	public void SetVelocityZero()

	{
		RB.velocity = Vector2.zero;
		CurrentVelocity = Vector2.zero;
	}
	public void SetVelocity(float velocity, Vector2 angle, int direction)
	{
		angle.Normalize();
		workspace.Set(angle.x * velocity * direction, angle.y * velocity);
		RB.velocity = workspace;
		CurrentVelocity = RB.velocity;
	}

	public void SetVelocity(float velocity, Vector2 direction)
	{
		workspace = direction * velocity;
		RB.velocity = workspace;
		CurrentVelocity = RB.velocity;
	}

	public void SetVelocityX(float velocity)
	{
		workspace.Set(velocity, CurrentVelocity.y);
		RB.velocity = workspace;
		CurrentVelocity = workspace;
	}

	public void SetVelocityY(float velocity)
	{
		workspace.Set(CurrentVelocity.x, velocity);
		RB.velocity = workspace;
		CurrentVelocity = workspace;
	}
	#endregion

	#region Check Functions

	public bool CheckIfTouchingCeiling()
	{
		return Physics2D.OverlapCircle(ceilingCheck.position, playerData.groundCheckRadius, playerData.whatisGround);
	}
	public bool CheckIfGrounded()
	{
		return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatisGround);
	}

	public bool CheckIfTouchingWall()
	{
		return Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, 
								playerData.wallCheckDistance, playerData.whatisGround);
	}

	public bool CheckIfTouchingLedge()
	{
		return Physics2D.Raycast(ledgeCheck.position, Vector2.right * FacingDirection,
			playerData.wallCheckDistance ,playerData.whatisGround);
	}

	public bool CheckIfTouchingWallBack()
	{
		return Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDirection,
								playerData.wallCheckDistance, playerData.whatisGround);
	}

	public void CheckIfShouldFlip(int xInput)
	{
		if(xInput != 0 && xInput != FacingDirection)
		{
			Flip();
		}
	}


	#endregion

	#region Other Functions

	public void SetColliderHeight(float height)
	{
		
		Vector2 center = MovementCollider.offset;
		workspace.Set(MovementCollider.size.x, height);

		center.y += (height - MovementCollider.size.y) / 2;

		
		MovementCollider.size = workspace;
		MovementCollider.offset = center;
		Debug.Log("size :" + MovementCollider.size.y);
		Debug.Log("offset :" + MovementCollider.offset.y);
	}

	public Vector2 DeterminCornerPosition()
	{
		RaycastHit2D xHit = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection,
			playerData.wallCheckDistance, playerData.whatisGround);

		float xDistance = xHit.distance;

		workspace.Set((xDistance + 0.015f) * FacingDirection, 0);
		RaycastHit2D yHit = Physics2D.Raycast(ledgeCheck.position + (Vector3)(workspace),
			Vector3.down, ledgeCheck.position.y - wallCheck.position.y + 0.015f, playerData.whatisGround);

		float yDistance = yHit.distance;

		Debug.Log("높이 : " + yDistance);
		Debug.Log("상태 : " + StateMachine.CurrentState);

		workspace.Set(wallCheck.position.x + (xDistance * FacingDirection),
			ledgeCheck.position.y - yDistance);

		return workspace;
	}


	private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

	private void LedgeClimb_0() => StateMachine.CurrentState.LedgeClimb_0();
	private void LedgeClimb_1() => StateMachine.CurrentState.LedgeClimb_1();
	private void LedgeClimb_2() => StateMachine.CurrentState.LedgeClimb_2();
	private void LedgeHold() => StateMachine.CurrentState.LedgeHold();
	private void LedgeGrab_0() => StateMachine.CurrentState.LedgeGrab_0();
	private void LedgeGrab_1() => StateMachine.CurrentState.LedgeGrab_1();
	private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

	private void Flip()
	{
		FacingDirection *= -1;
		transform.Rotate(0.0f, 180.0f, 0.0f);
	}
	#endregion

	public void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + FacingDirection * new Vector3(1, 0, 0));
		Gizmos.DrawLine(wallCheck.position, wallCheck.position + FacingDirection * new Vector3(1, 0, 0));
	}
}
