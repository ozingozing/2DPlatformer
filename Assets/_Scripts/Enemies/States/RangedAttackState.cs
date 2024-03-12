using Ozing.Projectile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozing.Weapons.ProjectileSpawnerStrategy;
using Ozing.Weapons.Components;
public class RangedAttackState : AttackState
{
	protected D_RangedAttackState stateData;


	protected GameObject projectile;
	protected Projectile projectileScript;
	public RangedAttackState(Entity entity, FiniteStateMachine sateMachine,
		string animBoolName, Transform attackPosition, D_RangedAttackState stateData) : base(entity, sateMachine, animBoolName, attackPosition)
	{
		this.stateData = stateData;
	}

	public override void DoChecks()
	{
		base.DoChecks();
	}

	public override void Enter()
	{
		base.Enter();
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void FinishAttack()
	{
		base.FinishAttack();
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}

	public override void TriggerAttack()
	{
		base.TriggerAttack();
	}
}
