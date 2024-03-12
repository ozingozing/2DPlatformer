using Ozing.CoreSystem;
using Ozing.Weapons.Components.ComponentData;
using Ozing.Weapons.Components.ComponentData.AttackData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozing.Weapons.Components
{
    public class KnockBack : WeaponComponent<KnockBackData, AttackKnockBack>
    {
        private ActionHitBox hitBox;
		private CoreSystem.Movement movement;
		private CoreSystem.Movement Movement => movement ? movement : Core.GetCoreComponent<CoreSystem.Movement>(ref movement);

		private void HandleDetectCollider2D(Collider2D[] colliders)
		{
			foreach (var item in colliders)
			{
				if (item.TryGetComponent(out IKnockBackable knockBackable))
				{
					knockBackable.KnockBack(new Combat.KnockBack.KnockBackData(currentAttackData.Angle, currentAttackData.Strength, Movement.FacingDirection, Core.Root));
				}
			}
		}

		protected override void Start()
		{
			base.Start();

			hitBox = GetComponent<ActionHitBox>();
			hitBox.OnDetectedCollider2D += HandleDetectCollider2D;
		}


		protected override void OnDestroy()
		{
			base.OnDestroy();

			hitBox.OnDetectedCollider2D -= HandleDetectCollider2D;
		}
	}
}
