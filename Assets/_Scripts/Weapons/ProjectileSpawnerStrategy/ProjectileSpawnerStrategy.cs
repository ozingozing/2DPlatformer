using Ozing.ObjectPoolSystem;
using Ozing.Weapons.Components.ComponentData.AttackData;
using System;
using System.Collections;
using UnityEditor.Connect;
using UnityEngine;

namespace Ozing.Weapons.ProjectileSpawnerStrategy
{
	public class ProjectileSpawnerStrategy : IProjectileSpawnerStrategy
	{
		private Vector2 spawnPos;
		private Vector2 spawnDir;
		private ProjectileSystem.Projectile currentProjectile;

		public virtual void ExecuteSpawnStrategy(
			ProjectileSpawnInfo projectileSpawnInfo,
			Vector3 spawnerPos,
			int facingDirection,
			ObjectPools objectPools,
			Action<ProjectileSystem.Projectile> OnSpawnProjectile)
		{
			SpawnProjectile(projectileSpawnInfo, projectileSpawnInfo.Direction, spawnerPos, facingDirection, objectPools, OnSpawnProjectile);
		}

		protected virtual void SpawnProjectile(
			ProjectileSpawnInfo projectileSpawnInfo,
			Vector2 spawnDirection,
			Vector3 spawnerPos,
			int facingDirection,
			ObjectPools objectPools,
			Action<ProjectileSystem.Projectile> OnSpawnProjectile
			)
		{
			SetSpawnPosition(spawnerPos, projectileSpawnInfo.Offset, facingDirection);
			SetSpawnDirection(spawnDirection, facingDirection);
			GetProjectileAndSetPositionAndRotation(objectPools, projectileSpawnInfo.ProjectilePrefab);
			InitializeProjectile(projectileSpawnInfo, OnSpawnProjectile);
		}

		protected virtual void GetProjectileAndSetPositionAndRotation(ObjectPools objectPools, ProjectileSystem.Projectile prefab)
		{
			currentProjectile = objectPools.GetObject(prefab);
			currentProjectile.transform.position = spawnPos;
			
			var angle = Mathf.Atan2(spawnDir.y, spawnDir.x) * Mathf.Rad2Deg;
			currentProjectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}

		protected virtual void InitializeProjectile(ProjectileSpawnInfo projectileSpawnInfo, Action<ProjectileSystem.Projectile> OnSpawnProjectile)
		{
			currentProjectile.Reset();

			currentProjectile.SendDataPackage(projectileSpawnInfo.DamageData);
			currentProjectile.SendDataPackage(projectileSpawnInfo.KnockBackData);
			currentProjectile.SendDataPackage(projectileSpawnInfo.PoiseDamageData);
			currentProjectile.SendDataPackage(projectileSpawnInfo.SpriteData);

			OnSpawnProjectile?.Invoke(currentProjectile);
			currentProjectile.Init();
		}

		protected virtual void SetSpawnDirection(Vector2 direction, int facingDirection)
		{
			spawnDir.Set(direction.x * facingDirection, direction.y);
		}

		protected virtual void SetSpawnPosition(Vector3 referencePosition, Vector2 offset, int facingDirection)
		{
			spawnPos = referencePosition;
			spawnPos.Set(spawnPos.x + offset.x * facingDirection, spawnPos.y + offset.y);
		}
	}
}