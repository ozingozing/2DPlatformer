using System;
using System.Collections;
using UnityEngine;

namespace Ozing.Assets._Scripts.Weapons
{
	public class Weapon : MonoBehaviour
	{
		public event Action OnExit;

		private Animator anim;
		private GameObject baseGO;

		private AnimationEventHandler eventHandler;

		public void Enter()
		{
			print($"{transform.name} enter");

			anim.SetBool("active", true);
		}

		private void Exit()
		{
			anim.SetBool("active", false);
			//Onexit에 연결되어있는 PlayerAttackState클래스의 ExitHandler함수를 실행시켜주기 위함
			OnExit?.Invoke();
		}

		private void Awake()
		{
			baseGO = transform.Find("Base").gameObject;
			anim = baseGO.GetComponent<Animator>();

			eventHandler = baseGO.GetComponent<AnimationEventHandler>();
		}

		private void OnEnable()
		{
			eventHandler.OnFinish += Exit;
		}

		private void OnDisable()
		{
			eventHandler.OnFinish -= Exit;
		}
	}
}