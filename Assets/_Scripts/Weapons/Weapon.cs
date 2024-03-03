using System;
using System.Collections;
using UnityEngine;
using Ozing.Utilities;
using Ozing.CoreSystem;
using Ozing.Weapons.Components.ComponentData;
using Ozing.Weapons.Components.ComponentData.AttackData;
using Ozing.Weapons.Components;

namespace Ozing.Weapons
{
	public class Weapon : MonoBehaviour
	{
		[SerializeField] private float attackCounterResetCooldown;

		[field: SerializeField] public WeaponDataSO Data { get; private set; }

		public int CurrentAttackCounter
		{
			get => currentAttackCounter;
			private set => currentAttackCounter = value >= Data.NumberOfAttacks ? 0 : value; 
		}
		private int currentAttackCounter;

		public event Action OnEnter;
		public event Action OnExit;

		private Animator anim;
		public GameObject BaseGO { get; private set; }
		public GameObject WeaponSpriteGO { get; private set; }
		public Core Core { get; private set; }
		public AnimationEventHandler EventHandler { get; private set; }

		private Timer attackCounterResetTimer;

		public void Enter()
		{
			print($"{transform.name} enter");

			attackCounterResetTimer.StopTimer();

			anim.SetBool("active", true);
			anim.SetInteger("counter", CurrentAttackCounter);

			OnEnter?.Invoke();
		}

		public void SetCore(Core core)
		{
			Core = core;
		}

		public void SetData(WeaponDataSO data)
		{
			Data = data;
		}

		private void Exit()
		{
			anim.SetBool("active", false);
			

			CurrentAttackCounter++;
			attackCounterResetTimer.StartTimer();
			//Onexit에 연결되어있는 PlayerAttackState클래스의 ExitHandler함수를 실행시켜주기 위함
			OnExit?.Invoke();
		}

		private void Update()
		{
			attackCounterResetTimer.Tick();
		}

		private void Awake()
		{
			BaseGO = transform.Find("Base").gameObject;
			WeaponSpriteGO = transform.Find("WeaponSprite").gameObject;
			anim = BaseGO.GetComponent<Animator>();

			EventHandler = BaseGO.GetComponent<AnimationEventHandler>();

			attackCounterResetTimer = new Timer(attackCounterResetCooldown);
		}

		private void ResetAttackCounter() => CurrentAttackCounter = 0;

		private void OnEnable()
		{
			EventHandler.OnFinish += Exit;
			attackCounterResetTimer.OnTimerDone += ResetAttackCounter;
		}

		private void OnDisable()
		{
			EventHandler.OnFinish -= Exit;
			attackCounterResetTimer.OnTimerDone -= ResetAttackCounter;
		}
	}
}