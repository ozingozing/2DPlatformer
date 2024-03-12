using Ozing.CoreSystem;
using Ozing.ObjectPoolSystem;
using Ozing.Weapons.Components.ComponentData.AttackData;
using Ozing.Weapons.ProjectileSpawnerStrategy;
using System;
using System.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class E2_RangedAttackState : RangedAttackState
{
	private Enemy2 enemy;


	public event Action<Ozing.ProjectileSystem.Projectile> OnSpawnProjectile;

	private Ozing.CoreSystem.Movement Movement => movement ? movement : core.GetCoreComponent(ref movement);
	private Ozing.CoreSystem.Movement movement;

	private readonly ObjectPools objectPools = new ObjectPools();

	private IProjectileSpawnerStrategy projectileSpawnerStrategy;

	public void SetprojectileSpawnerStrategy(IProjectileSpawnerStrategy newStrategy)
	{
		projectileSpawnerStrategy = newStrategy;
	}

	public E2_RangedAttackState(Entity entity, FiniteStateMachine sateMachine,
		string animBoolName, UnityEngine.Transform attackPosition, D_RangedAttackState stateData, Enemy2 enemy) : base(entity, sateMachine, animBoolName, attackPosition, stateData)
	{
		this.enemy = enemy;
		projectileSpawnerStrategy = new ProjectileSpawnerStrategy();
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

		if(isAnimationFinished)
		{
			if(isPlayerInMinAgroRange)
			{
				stateMachine.ChangeState(enemy.playerDetectedState);
			}
			else if(isPlayerInMaxAgroRange)
			{
				stateMachine.ChangeState(enemy.moveState);
			}
			else
			{
				stateMachine.ChangeState(enemy.lookForPlayerState);
			}
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}

	public override void TriggerAttack()
	{
		base.TriggerAttack();
		projectileSpawnerStrategy.ExecuteSpawnStrategy(
				enemy.ArrowData, attackPosition.position,
				Movement.FacingDirection, objectPools, OnSpawnProjectile
			);

		
	}
}
