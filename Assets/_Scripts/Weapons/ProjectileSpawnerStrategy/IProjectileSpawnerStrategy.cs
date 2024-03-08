using Ozing.ObjectPoolSystem;
using Ozing.Weapons.Components.ComponentData.AttackData;
using System;
using System.Collections;
using UnityEngine;

namespace Ozing.Weapons.ProjectileSpawnerStrategy
{
	public interface IProjectileSpawnerStrategy
	{
		void ExecuteSpawnStrategy(
				ProjectileSpawnInfo projectileSpawnInfo,
				Vector3 spawnerPos,
				int facingDirection,
				ObjectPools objectPools,
				Action<ProjectileSystem.Projectile> OnSpawnProjectile
			);
	}
}