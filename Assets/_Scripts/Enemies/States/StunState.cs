using Ozing.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
{
	protected Movement Movement
	{ get => movement ?? core.GetCoreComponent(ref movement); }
	private Movement movement;

	private CollisionSenses CollisionSenses
	{ get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
	private CollisionSenses collisionSenses;


	protected D_StunState stateData;

	protected bool isStunTimOver;
	protected bool isGrounded;
	protected bool isMovementStopped;
	protected bool performCloseRangeAction;
	protected bool isPlayerInMinAgroRange;

	public StunState(Entity entity, FiniteStateMachine sateMachine, string animBoolName, D_StunState stateData) : base(entity, sateMachine, animBoolName)
	{
		this.stateData = stateData;
	}

	public override void DoChecks()
	{
		base.DoChecks();

		if(CollisionSenses)	isGrounded = CollisionSenses.Ground;
		performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
		isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
	}

	public override void Enter()
	{
		base.Enter();

		isStunTimOver = false;
		isMovementStopped = false;
		Movement?.SetVelocity(stateData.stunKnockbackSpeed, stateData.stunKnockbackAngle,
							entity.lastDamageDirection);
	}

	public override void Exit()
	{
		base.Exit();
		entity.ResetStunResistance();
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		if(Time.time >= startTime + stateData.stunTime)
		{
			isStunTimOver = true;
		}

		if(isGrounded && Time.time >= startTime + stateData.stunKnockbackTime && !isMovementStopped)
		{
			isMovementStopped = true;
			Movement?.SetVelocityX(0f);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
}
