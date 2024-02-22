using Ozing.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeState : State
{
	protected Movement Movement
	{ get => movement ?? core.GetCoreComponent(ref movement); }
	private Movement movement;

	private CollisionSenses CollisionSenses
	{ get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
	private CollisionSenses collisionSenses;

	protected D_ChargeState stateData;

	protected bool isPlayerInMinAgroRange;
	protected bool isDetectingLedge;
	protected bool isDetectingWall;
	protected bool isChargeTimeOver;
	protected bool performCloseRangeAction;

	public ChargeState(Entity entity, FiniteStateMachine sateMachine, string animBoolName, D_ChargeState stateData) : base(entity, sateMachine, animBoolName)
	{
		this.stateData = stateData;
	}

	public override void DoChecks()
	{
		base.DoChecks();

		isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
		if(CollisionSenses)
		{
			isDetectingLedge = CollisionSenses.LedgeVertical;
			isDetectingWall = CollisionSenses.WallFront;
		}
		

		performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
	}

	public override void Enter()
	{
		base.Enter();

		isChargeTimeOver = false;
		Movement?.SetVelocityX(stateData.chargeSpeed * Movement.FacingDirection);
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		Movement?.SetVelocityX(stateData.chargeSpeed * Movement.FacingDirection);

		if (Time.time >= startTime + stateData.chargeTime)
		{
			isChargeTimeOver = true;
		}
		
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
}
