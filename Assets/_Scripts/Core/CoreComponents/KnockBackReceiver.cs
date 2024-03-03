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

		private Movement movement;
		private CollisionSenses collisionSenses;

		public override void LogicUpdate()
		{
			CheckKnockBack();
		}

		public void KnockBack(Vector2 angle, float strength, int direction)
		{
			movement?.SetVelocity(strength, angle, direction);
			movement.canSetVelocity = false;
			isKnockBackActive = true;
			knockBackStartTime = Time.time;
		}

		private void CheckKnockBack()
		{
			if (isKnockBackActive
				&& movement?.CurrentVelocity.y <= 0.01f
				&& collisionSenses.Ground
				|| Time.time >= knockBackStartTime + maxKnockBackTime)
			{
				isKnockBackActive = false;
				movement.canSetVelocity = true;
			}
		}

		protected override void Awake()
		{
			base.Awake();

			movement = core.GetCoreComponent<Movement>();
			collisionSenses = core.GetCoreComponent<CollisionSenses>();
		}
	}
}