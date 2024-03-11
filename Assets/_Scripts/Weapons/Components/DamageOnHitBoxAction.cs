using Ozing.Weapons.Components.ComponentData;
using Ozing.Weapons.Components.ComponentData.AttackData;
using System.Collections;
using System.Collections.Generic;
using Ozing.Utilities;
using UnityEngine;

namespace Ozing.Weapons.Components
{
    public class DamageOnHitBoxAction : WeaponComponent<DamageOnHitBoxActionData, AttackDamage>
    {
        private ActionHitBox hitBox;


        private void HandleDetectCollider2D(Collider2D[] colliders)
        {
			CombatDamageUtility.TryDamage(colliders, new Combat.Damage.DamageData(currentAttackData.Amount, Core.Root), out _);
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
