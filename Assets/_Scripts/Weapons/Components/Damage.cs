using Ozing.Weapons.Components.ComponentData;
using Ozing.Weapons.Components.ComponentData.AttackData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozing.Weapons.Components
{
    public class Damage : WeaponComponent<DamageData, AttackDamage>
    {
        private ActionHitBox hitBox;


        private void HandleDetectCollider2D(Collider2D[] colliders)
        {
            foreach (var item in colliders)
            {
                if(item.TryGetComponent(out IDamageable damageable))
				{
					damageable.Damage(currentAttackData.Amount);
				}
            }
        }

		protected override void Awake()
		{
			base.Awake();

            hitBox = GetComponent<ActionHitBox>();
		}

		protected override void OnEnable()
		{
			base.OnEnable();

            hitBox.OnDetectedCollider2D += HandleDetectCollider2D;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			hitBox.OnDetectedCollider2D -= HandleDetectCollider2D;
		}
	}
}
