using Ozing.Assets._Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
	private Weapon weapon;
	public PlayerAttackState(
		Player player,
		PlayerStateMachine stateMachine,
		PlayerData playerData,
		string animBoolName,
		Weapon weapon
	) : base(player, stateMachine, playerData, animBoolName)
	{
		this.weapon = weapon;

		weapon.OnExit += ExitHandler;
	}

	public override void Enter()
	{
		base.Enter();

		weapon.Enter();
	}

	private void ExitHandler()
	{
		base.AnimationFinishTrigger();
		isAbilityDone = true;
	}
}
