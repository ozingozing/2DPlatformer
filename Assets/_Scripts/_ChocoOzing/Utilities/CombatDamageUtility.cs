using Ozing.Combat.Damage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozing.Utilities
{
	public class CombatDamageUtility
	{
		public static bool TryDamage(GameObject gameObject, DamageData damageData, out IDamageable damageable)
		{
			if(gameObject.TryGetComponentInChildren(out damageable))
			{
				damageable.Damage(damageData);
				return true;
			}
			return false;
		}

		public static bool TryDamage(Collider2D[] colliders, DamageData damageData, out List<IDamageable> damageables)
		{
			var hashDamaged = false;
			damageables = new List<IDamageable>();

			foreach (var collider in colliders)
			{
				if(TryDamage(collider.gameObject, damageData, out IDamageable damageable))
				{
					damageables.Add(damageable);
					return true;
				}
			}

			return hashDamaged;
		}
	}
}