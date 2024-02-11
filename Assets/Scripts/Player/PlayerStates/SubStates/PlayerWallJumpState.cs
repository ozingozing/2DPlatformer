using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
	private int wallJumpDirection;
	private Vector2 currentVelocity;

	public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine,
		PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();

		currentVelocity = player.CurrentVelocity;

		player.InputHandler.UseJumpInput();
		player.JumpState.DecreaseAmountOfJumpsLeft();
		player.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle, wallJumpDirection);
		player.CheckIfShouldFlip(wallJumpDirection);
		player.JumpState.DecreaseAmountOfJumpsLeft();
	}

	public override void Exit()
	{
		base.Exit();
		player.SetVelocity(0, playerData.wallJumpAngle, wallJumpDirection);
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
		player.Anim.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));

		if(Time.time >= startTime + playerData.wallJumpTime)
		{
			isAbilityDone = true;
		}
	}

	public void DeterminWallJumpDirection(bool isTouchingWall)
	{
		if(isTouchingWall)
		{
			wallJumpDirection = -player.FacingDirection;
		}
		else
		{
			wallJumpDirection = player.FacingDirection;
		}
	}

	
}
