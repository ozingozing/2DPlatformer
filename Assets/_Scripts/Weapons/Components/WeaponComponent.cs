﻿using Ozing.CoreSystem;
using System.Collections;
using UnityEngine;

namespace Ozing.Weapons.Components
{
	public abstract class WeaponComponent : MonoBehaviour
	{
		protected Weapon weapon;

		protected AnimationEventHandler EventHandler;// => weapon.EventHandler;
		protected Core Core => weapon.Core;

		protected bool isAttackActive;

		protected virtual void Awake()
		{
			weapon = GetComponent<Weapon>();
			EventHandler = GetComponentInChildren<AnimationEventHandler>();
		}

		protected virtual void HandleEnter()
		{
			isAttackActive = true;
		}

		protected virtual void HandleExit()
		{
			isAttackActive = false;
		}

		protected virtual void OnEnable()
		{
			weapon.OnEnter += HandleEnter;
			weapon.OnExit += HandleExit;
		}

		protected virtual void OnDisable()
		{
			weapon.OnEnter -= HandleEnter;
			weapon.OnExit -= HandleExit;
		}
	}
}