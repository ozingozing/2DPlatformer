using Ozing.CoreSystem;
using Ozing.Weapons.Components;
using Ozing.Weapons.Components.ComponentData;
using Ozing.Weapons.Components.ComponentData.AttackData;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Ozing.Weapons.Components
{
	public abstract class WeaponComponent : MonoBehaviour
	{
		protected Weapon weapon;

		protected AnimationEventHandler eventHandler;// => weapon.EventHandler;
		protected Core Core => weapon.Core;

		protected bool isAttackActive;

		public virtual void Init()
		{

		}

		protected virtual void Awake()
		{
			weapon = GetComponent<Weapon>();
			eventHandler = GetComponentInChildren<AnimationEventHandler>();
		}

		protected virtual void Start()
		{
			weapon.OnEnter += HandleEnter;
			weapon.OnExit += HandleExit;
		}


		protected virtual void HandleEnter()
		{
			isAttackActive = true;
		}

		protected virtual void HandleExit()
		{
			isAttackActive = false;
		}

		protected virtual void OnDestroy()
		{
			weapon.OnEnter -= HandleEnter;
			weapon.OnExit -= HandleExit;
		}
	}
}

public abstract class WeaponComponent<T1, T2> : WeaponComponent where T1 : ComponentData<T2> where T2 : AttackData
{
	protected T1 data;
	protected T2 currentAttackData;

	protected override void HandleEnter()
	{
		base.HandleEnter();
		currentAttackData = data.AttackData[weapon.CurrentAttackCounter];
	}

	public override void Init()
	{
		base.Init();

		data = weapon.Data.GetData<T1>();
	}
}