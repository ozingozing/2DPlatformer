using Ozing.Weapons.Components.ComponentData.AttackData;
using System.Collections;
using UnityEngine;

namespace Ozing.Weapons.Components.ComponentData
{
	public class ProjectileSpawnerData : ComponentData<AttackProjectileSpawner>
	{
		protected override void SetComponentDependency()
		{
			ComponentDependency = typeof(ProjectileSpawner);
		}
	}
}