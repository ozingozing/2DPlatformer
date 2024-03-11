using Ozing.Weapons.Components.ComponentData.AttackData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozing.Weapons.Components.ComponentData
{
	public class DamageOnHitBoxActionData : ComponentData<AttackDamage>
	{
		protected override void SetComponentDependency()
		{
			ComponentDependency = typeof(DamageOnHitBoxAction);
		}
	}
}
