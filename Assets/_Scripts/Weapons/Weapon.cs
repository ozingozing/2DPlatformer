using System;
using System.Collections;
using UnityEngine;
using Ozing.Assets._Scripts._ChocoOzing.Utilities;

namespace Ozing.Assets._Scripts.Weapons
{
	public class Weapon : MonoBehaviour
	{
		[SerializeField] private int numberOfAttacks;
		[SerializeField] private float attackCounterResetCooldown;
		
		public int CurrentAttackCounter
		{
			get => currentAttackCounter;
			private set => currentAttackCounter = value >= numberOfAttacks ? 0 : value; 
		}
		private int currentAttackCounter;

		public event Action OnEnter;
		public event Action OnExit;

		private Animator anim;
		public GameObject BaseGO { get; private set; }
		public GameObject WeaponSpriteGO { get; private set; }

		private AnimationEventHandler eventHandler;

		private Timer attackCounterResetTimer;

		public void Enter()
		{
			print($"{transform.name} enter");

			attackCounterResetTimer.StopTimer();

			anim.SetBool("active", true);
			anim.SetInteger("counter", CurrentAttackCounter);

			OnEnter?.Invoke();
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

			eventHandler = BaseGO.GetComponent<AnimationEventHandler>();

			attackCounterResetTimer = new Timer(attackCounterResetCooldown);
		}

		private void ResetAttackCounter() => CurrentAttackCounter = 0;

		private void OnEnable()
		{
			eventHandler.OnFinish += Exit;
			attackCounterResetTimer.OnTimerDone += ResetAttackCounter;
		}

		private void OnDisable()
		{
			eventHandler.OnFinish -= Exit;
			attackCounterResetTimer.OnTimerDone -= ResetAttackCounter;
		}
	}
}