using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class WeaponAnitionToWeapon : MonoBehaviour
{
	private Weapon weapon;


	private void Start()
	{
		//AnimationToStatemachine���� AttackState���� ������ �ʱ�ȭ�� ���༭ �� ������
		//���⼱ �ٸ� ������ �ʱ�ȭ�� �� �����༭ ���⼭ ������ �ʱ�ȭ�� ����
		weapon = GetComponentInParent<Weapon>();
	}

	private void AnimationFinishTrigger()
	{
		weapon.AnimationFinishTrigger();
	}

	private void AnimationStartMovementTrigger()
	{
		weapon.AnimationStartMovementTrigger();
	}

	private void AnimationStopMovementTrigger()
	{
		weapon.AnimationStopMovementTrigger();
	}

	private void AnimationTurnOffFlipTrigger()
	{
		weapon.AnimationTurnOffFlip();
	}

	private void AnimationTurnOnFlipTrigger()
	{
		weapon.AnimationTurnOnFlip();
	}

	private void AnimationActionTigger()
	{
		weapon.AnimationActionTrigger();
	}
}
