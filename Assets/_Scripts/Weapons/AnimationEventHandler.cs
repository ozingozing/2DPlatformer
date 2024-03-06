using System;
using System.Collections;
using UnityEngine;

namespace Ozing.Weapons
{
	public class AnimationEventHandler : MonoBehaviour
	{
		public event Action OnFinish;
		public event Action OnStartMovement;
		public event Action OnStopMovement;
		public event Action OnAttackAction;
		public event Action<AttackPhases> OnEnterAttackPhase;
		public event Action OnMinHoldPassed;
		private void AnimationFinishedTrigger() => OnFinish?.Invoke();
		private void StartMovementTrigger() => OnStartMovement?.Invoke();
		private void StopMovementTigger() => OnStopMovement?.Invoke();
		private void AttackActionTrigger() => OnAttackAction?.Invoke();
		private void MinHoldPassedTrigger() => OnMinHoldPassed?.Invoke();
		private void EnterAttackPhase(AttackPhases phase) => OnEnterAttackPhase?.Invoke(phase);
		
	}
}