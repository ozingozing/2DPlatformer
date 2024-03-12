using Ozing.Combat.KnockBack;
using Ozing.ModifireSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozing.CoreSystem
{
	public class KnockBackReceiver : CoreComponent, IKnockBackable
	{
		public Modifiers<KnockBackData> Modifiers { get; } = new();

		[SerializeField] private float maxKnockBackTime = 0.2f;

		private bool isKnockBackActive;
		private float knockBackStartTime;

		private Movement Movement => movement ? movement : core.GetCoreComponent(ref movement);
		private Movement movement;
		private CollisionSenses CollisionSenses => collisionSenses ? collisionSenses : core.GetCoreComponent(ref collisionSenses);
		private CollisionSenses collisionSenses;

		public override void LogicUpdate()
		{
			CheckKnockBack();
		}

		public void KnockBack(KnockBackData data)
		{
			data = Modifiers.ApplyAllModifiers(data);

			Movement?.SetVelocity(data.Strength, data.Angle, data.Direction);
			Movement.canSetVelocity = false;
			isKnockBackActive = true;
			knockBackStartTime = Time.time;
		}

		private void CheckKnockBack()
		{
			if (isKnockBackActive
				&& Movement?.CurrentVelocity.y <= 0.01f
				&& CollisionSenses.Ground
				|| Time.time >= knockBackStartTime + maxKnockBackTime)
			{
				isKnockBackActive = false;
				Movement.canSetVelocity = true;
			}
		}

		protected override void Awake()
		{
			base.Awake();

			//movement = core.GetCoreComponent<Movement>();
			//collisionSenses = core.GetCoreComponent<CollisionSenses>();
		}

		
	}
}