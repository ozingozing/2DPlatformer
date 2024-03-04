using Ozing.Weapons.Components.ComponentData.AttackData;
using System.Collections;
using UnityEngine;

namespace Ozing.Weapons.Components.ComponentData
{
	public class PoiseDamageData : ComponentData<AttackPoiseDamage>
	{
		protected override void SetComponentDependency()
		{
			ComponentDependency = typeof(PoiseDamage);
		}
	}
}