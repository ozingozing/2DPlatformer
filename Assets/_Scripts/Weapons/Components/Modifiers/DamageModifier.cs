using Ozing.Combat.Damage;
using Ozing.ModifireSystem;
using System;
using System.Collections;
using UnityEngine;


namespace Ozing.Weapons.Components.Modifiers
{
	public class DamageModifier : Modifier<DamageData>
	{
		public event Action<GameObject> OnModified;

		private readonly ConditionalDelegate isBlocked;

		public DamageModifier(ConditionalDelegate value)
		{
			this.isBlocked = value;
		}

		public override DamageData ModifyValue(DamageData value)
		{
			if(isBlocked(value.Source.transform, out var blockDirectionInformation))
			{
				value.SetAmount(value.Amount * (1 - blockDirectionInformation.DamageAbsorption));
				OnModified?.Invoke(value.Source);
			}

			return value;
		}
	}
}