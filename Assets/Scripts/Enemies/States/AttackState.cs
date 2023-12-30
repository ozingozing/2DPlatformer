using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
	protected Transform attackPosition;

	protected bool isAnimationFinished;
	protected bool isPlayerInMinAgroRange;

	public AttackState(Entity entity, FiniteStateMachine sateMachine, string animBoolName, Transform attackPosition) : base(entity, sateMachine, animBoolName)
	{
		this.attackPosition = attackPosition;
	}

	public override void DoChecks()
	{
		base.DoChecks();

		isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
	}

	public override void Enter()
	{
		base.Enter();

		entity.atsm.attackState = this;
		isAnimationFinished = false;
		entity.SetVelocity(0);
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}

	public virtual void TriggerAttack()
	{

	}

	public virtual void FinishAttack()
	{
		isAnimationFinished = true;
	}
}
