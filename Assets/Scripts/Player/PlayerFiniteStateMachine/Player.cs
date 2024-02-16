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
	public Core Core { get; private set; }
	public Animator Anim { get; private set; }
	public PlayerInputHandler InputHandler { get; private set; }
	public Rigidbody2D RB { get; private set; }
	public Transform DashDirectionIndicator { get; private set; }
	public Transform Weapon {  get; private set; }
	public BoxCollider2D MovementCollider { get; private set; }
	public PlayerInventory Inventory { get; private set; }

	#endregion



	#region Other Variables

	private Vector2 workspace;
	#endregion

	#region Unity Callback Functions
	private void Awake()
	{
		Core = GetComponentInChildren<Core>();

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
		DashDirectionIndicator = transform.GetChild(1);
		Weapon = transform.GetChild(2);
		Weapon.gameObject.SetActive(true);
		MovementCollider = GetComponent<BoxCollider2D>();
		Inventory = GetComponent<PlayerInventory>();


		PrimaryAttackState.SetWeapon(Inventory.weapons[(int)CombatInputs.primary]);
		//SecondaryAttackState.SetWeapon(Inventory.weapons[(int)CombatInputs.secondary]);

		StateMachine.Initialize(IdleState);
	}

	private void Update()
	{
		Core.LogicUpdate();
		StateMachine.CurrentState.LogicUpdate();
	}

	private void FixedUpdate()
	{
		StateMachine.CurrentState.PhysicsUpdate();
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

	


	private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();
	private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
	private void LedgeClimb_0() => StateMachine.CurrentState.LedgeClimb_0();
	private void LedgeClimb_1() => StateMachine.CurrentState.LedgeClimb_1();
	private void LedgeClimb_2() => StateMachine.CurrentState.LedgeClimb_2();
	private void LedgeHold() => StateMachine.CurrentState.LedgeHold();
	private void LedgeGrab_0() => StateMachine.CurrentState.LedgeGrab_0();
	private void LedgeGrab_1() => StateMachine.CurrentState.LedgeGrab_1();

	
	#endregion

	public void OnDrawGizmos()
	{
		if(Core != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(Core.CollisionSenses.LedgeCheckHorizontal.position,
				Core.CollisionSenses.LedgeCheckHorizontal.position + Core.Movement.FacingDirection * new Vector3(1, 0, 0));
			Gizmos.DrawLine(Core.CollisionSenses.WallCheck.position,
				Core.CollisionSenses.WallCheck.position + Core.Movement.FacingDirection * new Vector3(1, 0, 0));

		}
	}
}
