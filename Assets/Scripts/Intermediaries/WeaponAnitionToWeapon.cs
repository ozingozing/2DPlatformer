using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class WeaponAnitionToWeapon : MonoBehaviour
{
	private Weapon weapon;

	private void Start()
	{
		//AnimationToStatemachine에선 AttackState에서 별도로 초기화를 해줘서 안 썼지만
		//여기선 다른 곳에서 초기화를 안 시켜줘서 여기서 별도로 초기화를 해줌
		weapon = GetComponentInParent<Weapon>();
	}

	private void AnimationFinishTrigger()
	{
		weapon.AnimationFinishTrigger();
	}
}
