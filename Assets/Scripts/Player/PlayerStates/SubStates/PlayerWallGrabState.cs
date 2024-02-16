using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrabState : PlayerTouchingWallState
{
	private Vector2 holdPostion;
	public PlayerWallGrabState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
	{
	}

	public override void AnimationFinishTrigger()
	{
		base.AnimationFinishTrigger();
	}

	public override void AnimationTrigger()
	{
		base.AnimationTrigger();
	}

	public override void DoChecks()
	{
		base.DoChecks();
	}

	public override void Enter()
	{
		base.Enter();

		holdPostion = player.transform.position;

		HoldPosition();
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		

		if(!isExitingState)
		{
			HoldPosition();

			if (yInput > 0)
			{
				stateMachine.ChangeState(player.WallClimbState);
			}
			else if (yInput < 0 || !grabInput)
			{
				stateMachine.ChangeState(player.WallSlideState);
			}
		}
	}

	private void HoldPosition()
	{
		player.transform.position = holdPostion;

		core.Movement.SetVelocityY(0);
		core.Movement.SetVelocityX(0);
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
}
