using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozing.CoreSystem
{
	public class KnockBackReceiver : CoreComponent, IKnockBackable
	{

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

		public void KnockBack(Vector2 angle, float strength, int direction)
		{
			Movement?.SetVelocity(strength, angle, direction);
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