using Ozing.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : State
{
	protected Movement Movement
	{ get => movement ?? core.GetCoreComponent(ref movement); }
	private Movement movement;

	private CollisionSenses CollisionSenses
	{ get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
	private CollisionSenses collisionSenses;


	protected D_PlayerDetected stateData;
	protected bool isPlayerInMinAgroRange;
	protected bool isPlayerInMaxAgroRange;
	protected bool performLongRangeAction;
	protected bool performCloseRangeAction;
	protected bool isDetectingLedge;

	public PlayerDetectedState(Entity entity, FiniteStateMachine sateMachine, string animBoolName, D_PlayerDetected stateData) : base(entity, sateMachine, animBoolName)
	{
		this.stateData = stateData;
	}

	public override void DoChecks()
	{
		base.DoChecks();
		isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
		isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
		if(CollisionSenses)	isDetectingLedge = CollisionSenses.LedgeVertical;

		performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
	}

	public override void Enter()
	{
		base.Enter();

		performLongRangeAction = false;
		Movement?.SetVelocityX(0);

	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		//core.Movement.SetVelocityX(0);

		if (Time.time >= startTime + stateData.longRangeActionTime)
		{
			performLongRangeAction = true;
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
}
