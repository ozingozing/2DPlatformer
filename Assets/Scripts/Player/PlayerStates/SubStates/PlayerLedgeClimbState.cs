using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
	private Vector2 detectedPos;
	public Vector2 cornerPos;
	private Vector2 startPos;
	private Vector2 stopPos;

	private bool isHanging;
	private bool isClimbing;
	private bool jumpInput;
	private bool isTouchingCeiling;

	private int xInput;
	private int yInput;


	public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine,
		PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
	{
	}

	public override void LedgeClimb_0() { player.MovementCollider.size = new Vector2(0.6f, 1.3f); player.MovementCollider.offset = new Vector2(0, 0.7f); }
	public override void LedgeClimb_1() { player.MovementCollider.size = new Vector2(0.7f, 0.6f); player.MovementCollider.offset = new Vector2(0.6f, 1.3f); }
	public override void LedgeClimb_2() { player.MovementCollider.size = new Vector2(0.7f, 1.2f); player.MovementCollider.offset = new Vector2(0.6f, 1.6f); }
	public override void LedgeHold() { player.MovementCollider.size = new Vector2(0.6f, 1.6f); player.MovementCollider.offset = new Vector2(0, 0.19f); }
	public override void LedgeGrab_0() { player.MovementCollider.size = new Vector2(0.6f, 1.6f); player.MovementCollider.offset = new Vector2(-0.12f, 0.19f); }
	public override void LedgeGrab_1() { player.MovementCollider.size = new Vector2(0.6f, 1.6f); player.MovementCollider.offset = new Vector2(0, 0.19f); }
	public override void AnimationFinishTrigger()
	{
		base.AnimationFinishTrigger();
		
		player.Anim.SetBool("climbLedge", false);
	}

	public override void AnimationTrigger()
	{
		base.AnimationTrigger();
		isHanging = true;
	}

	public override void Enter()
	{
		base.Enter();

		Debug.DrawRay(player.wallCheck.position, Vector3.right * player.FacingDirection, Color.blue, playerData.standColliderHeight);
		Debug.DrawRay(player.ledgeCheck.position, Vector3.right * player.FacingDirection, Color.blue, playerData.standColliderHeight);

		if (stateMachine.CurrentState == (player.LedgeClimbState)
			|| player.CheckIfTouchingWall()
			|| stateMachine.CurrentState == (player.WallClimbState)
			|| stateMachine.CurrentState == (player.WallGrabState))
		{
			//RayCastCheck
			Debug.DrawRay(player.DeterminCornerPosition() + (Vector2.up * 0.015f) + (Vector2.right * player.FacingDirection * 0.015f),
													Vector2.up, Color.magenta, playerData.standColliderHeight);
		}

		player.SetVelocityZero();
		cornerPos = player.DeterminCornerPosition();


		startPos.Set(cornerPos.x - (player.FacingDirection * playerData.startOffset.x),
			cornerPos.y - playerData.startOffset.y);
		stopPos.Set(cornerPos.x + (player.FacingDirection * playerData.stopOffset.x),
			cornerPos.y + playerData.stopOffset.y);

		player.transform.position = startPos;
	}

	public override void Exit()
	{
		base.Exit();

		isHanging = false;

		if(isClimbing)
		{
			player.transform.position = stopPos;
			isClimbing = false;
		}
		player.MovementCollider.size = new Vector2(0.6f, 1.6f);
		player.MovementCollider.offset = new Vector2(0, -0.16f);
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		

		if (isAnimationFinished)
		{
			if(isTouchingCeiling)
			{
				stateMachine.ChangeState(player.CrouchIdleState);
			}
			else
			{
				stateMachine.ChangeState(player.IdleState);
			}
		}
		else
		{
			xInput = player.InputHandler.NormalInputX;
			yInput = player.InputHandler.NormalInputY;
			jumpInput = player.InputHandler.JumpInput;

			player.SetVelocityZero();
			player.transform.position = startPos;

			if (xInput == player.FacingDirection && isHanging && !isClimbing)
			{
				CheckForSpace();
				isClimbing = true;
				player.Anim.SetBool("climbLedge", true);
			}
			else if ((yInput == -1) && isHanging && !isClimbing)
			{
				stateMachine.ChangeState(player.InAirState);
			}
			else if(jumpInput && !isClimbing)
			{
				player.WallJumpState.DeterminWallJumpDirection(true);
				stateMachine.ChangeState(player.WallJumpState);
			}
		}
	}

	public void SetDetectedPosition(Vector2 pos) => detectedPos = pos;

	private void CheckForSpace()
	{
		isTouchingCeiling = Physics2D.Raycast(cornerPos + (Vector2.up * 0.015f) + (Vector2.right * player.FacingDirection * 0.015f),
												Vector2.up, playerData.standColliderHeight, playerData.whatisGround);
		player.Anim.SetBool("isTouchingCeiling", isTouchingCeiling);
	}
}
