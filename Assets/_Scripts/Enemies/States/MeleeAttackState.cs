using Ozing.CoreSystem;
using Ozing.Combat.KnockBack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MeleeAttackState : AttackState
{
	protected Movement Movement
	{ get => movement ?? core.GetCoreComponent(ref movement); }
	private Movement movement;

	private CollisionSenses CollisionSenses
	{ get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
	private CollisionSenses collisionSenses;


	protected D_MeleeAttack stateData;


	public MeleeAttackState(Entity entity, FiniteStateMachine sateMachine, string animBoolName, Transform attackPosition, D_MeleeAttack stateData) : base(entity, sateMachine, animBoolName, attackPosition)
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

		Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.whatIsPlayer);

		foreach(Collider2D collider in detectedObjects)
		{
			IDamageable damageable = collider.GetComponent<IDamageable>();

			if(damageable != null)
			{
				damageable.Damage(new Ozing.Combat.Damage.DamageData(stateData.attackDamage, core.Root));
			}

			IKnockBackable knockbackable = collider.GetComponent<IKnockBackable>();

			if(knockbackable != null)
			{
				knockbackable.KnockBack(
						new KnockBackData(stateData.knockbackAngle, stateData.knockbackStrength, Movement.FacingDirection, core.Root)
					);
			}
		}
	}
}
