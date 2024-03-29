using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_MeleeAttackState : MeleeAttackState
{
	private Enemy2 enemy;
	public E2_MeleeAttackState(Entity entity, FiniteStateMachine sateMachine,
		string animBoolName, Transform attackPosition, D_MeleeAttack stateData, Enemy2 enemy) : base(entity, sateMachine, animBoolName, attackPosition, stateData)
	{
		this.enemy = enemy;
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
				stateMachine.ChangeState(enemy.dodgeState);
			}
			else if(!isPlayerInMinAgroRange)
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
	}
}
