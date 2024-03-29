using Cinemachine.Utility;
using Ozing.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
	//Input
	private int xInput;
	private bool jumpInput;
	private bool jumpInputStop;
	private bool grabInput;
	private bool dashInput;

	//Checks
	private bool isGrounded;
	private bool isTouchingWall;
	private bool isTouchingWallBack;
	private bool oldTouchingWall;
	private bool oldTouchingWallBack;
	private bool isTouchingLedge;


	private bool coyoteTime;
	private bool wallJumpCoyoteTime;
	private bool isJumping;
	
	private float startWallJumpCoyoteTime;

	protected Movement Movement
	{ get => movement ?? core.GetCoreComponent(ref movement); }
	private Movement movement;

	private CollisionSenses CollisionSenses
	{ get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
	private CollisionSenses collisionSenses;


	public PlayerInAirState(Player player, PlayerStateMachine stateMachine,
		PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
	{
	}

	public override void DoChecks()
	{
		base.DoChecks();

		oldTouchingWall = isTouchingWall;
		oldTouchingWallBack = isTouchingWallBack;

		if(CollisionSenses)
		{
			isGrounded = CollisionSenses.Ground;
			isTouchingWall = CollisionSenses.WallFront;
			isTouchingWallBack = CollisionSenses.WallBack;
			isTouchingLedge = CollisionSenses.LedgeHorizontal;
		}


		if (isTouchingWall && !isTouchingLedge && !isGrounded)
		{
			player.LedgeClimbState.SetDetectedPosition(player.transform.position);
		}

		if(!wallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack
			&&(oldTouchingWall || oldTouchingWallBack))
		{
			StartWallJumpCoyoteTime();
		}
	}

	public override void Enter()
	{
		base.Enter();
	}

	public override void Exit()
	{
		base.Exit();

		oldTouchingWall = false;
		oldTouchingWallBack = false;
		isTouchingWall = false;
		isTouchingWallBack = false;
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		CheckCoyoteTime();
		CheckWallJumpCoyoteTime();

		xInput = player.InputHandler.NormalInputX;
		jumpInput = player.InputHandler.JumpInput;
		jumpInputStop = player.InputHandler.JumpInputStop;
		grabInput = player.InputHandler.GrabInput;
		dashInput = player.InputHandler.DashInput;

		CheckJumpMultiplier();

		if (player.InputHandler.AttackInputs[(int)CombatInputs.primary])
		{
			stateMachine.ChangeState(player.PrimaryAttackState);
		}
		else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondary])
		{
			stateMachine.ChangeState(player.SecondaryAttackState);
		}
		else if (isGrounded && Movement?.CurrentVelocity.y < 0.01f)
		{
			stateMachine.ChangeState(player.IdleState);
		}
		else if (jumpInput && (isTouchingWall || isTouchingWallBack || wallJumpCoyoteTime))
		{
			StopWallJumpCoyoteTime();
			isTouchingWall = CollisionSenses.WallFront;
			player.WallJumpState.DeterminWallJumpDirection(isTouchingWall);
			stateMachine.ChangeState(player.WallJumpState);
		}
		else if(jumpInput && player.JumpState.CanJump())
		{
			stateMachine.ChangeState(player.JumpState);
		}
		else if(isTouchingWall && grabInput)
		{
			stateMachine.ChangeState(player.WallGrabState);
		}
		else if(isTouchingWall && xInput == Movement?.FacingDirection
			&& Movement?.CurrentVelocity.y <= 0)
		{
			stateMachine.ChangeState(player.WallSlideState);
		}
		else if(dashInput && player.DashState.CheckIfCanDash())
		{
			stateMachine.ChangeState(player.DashState);
		}
		else
		{
			Movement?.CheckIfShouldFlip(xInput);
			Movement?.SetVelocityX(playerData.movementVelocity * xInput);

			player.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
			player.Anim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));
		}
	}

	private void CheckJumpMultiplier()
	{
		if (isJumping)
		{
			if (jumpInputStop)
			{
				Movement?.SetVelocityY(Movement.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
				isJumping = false;
			}
			else if (Movement?.CurrentVelocity.y <= 0f)
			{
				isJumping = false;
			}
		}
	}


	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
		
		if (isTouchingWall && !isTouchingLedge && !isGrounded)
		{
			CheckJumpMultiplier();
			stateMachine.ChangeState(player.LedgeClimbState);
		}
	}

	private void CheckCoyoteTime()
	{
		if(coyoteTime && Time.time > startTime + playerData.coyoteTime)
		{
			coyoteTime = false;
			player.JumpState.DecreaseAmountOfJumpsLeft();
		}
	}

	private void CheckWallJumpCoyoteTime()
	{
		if(wallJumpCoyoteTime && Time.time > startWallJumpCoyoteTime + playerData.coyoteTime)
		{
			wallJumpCoyoteTime = false;
		}
	}

	public void StartCoyoteTime() => coyoteTime = true;

	public void StartWallJumpCoyoteTime()
	{
		wallJumpCoyoteTime = true;
		startWallJumpCoyoteTime = Time.time;
	}

	public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;

	public void SetIsJumping() => isJumping = true;
}
