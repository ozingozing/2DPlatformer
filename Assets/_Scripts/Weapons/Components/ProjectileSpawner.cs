using Ozing.Weapons.Components.ComponentData;
using Ozing.Weapons.Components.ComponentData.AttackData;
using Ozing.ObjectPoolSystem;
using Ozing.ProjectileSystem;
using System.Collections;
using UnityEngine;
using System;
using Ozing.Weapons.ProjectileSpawnerStrategy;

namespace Ozing.Weapons.Components
{
	public class ProjectileSpawner : WeaponComponent<ProjectileSpawnerData, AttackProjectileSpawner>
	{
		public event Action<ProjectileSystem.Projectile> OnSpawnProjectile;

		private CoreSystem.Movement Movement => movement ? movement : Core.GetCoreComponent(ref movement);
		private CoreSystem.Movement movement;

		private readonly ObjectPools objectPools = new ObjectPools();

		private IProjectileSpawnerStrategy projectileSpawnerStrategy;

		public void SetprojectileSpawnerStrategy(IProjectileSpawnerStrategy newStrategy)
		{
			projectileSpawnerStrategy = newStrategy;
		}

		private void HandleAttackAction()
		{
			foreach (var projectileSpawnInfo in currentAttackData.SpawnInfos)
			{
				projectileSpawnerStrategy.ExecuteSpawnStrategy(projectileSpawnInfo, transform.position, Movement.FacingDirection, objectPools, OnSpawnProjectile);
			}
		}

		private void SetDefualtProjectileSpawnStrategy()
		{
			projectileSpawnerStrategy = new ProjectileSpawnerStrategy.ProjectileSpawnerStrategy();
		}

		protected override void HandleExit()
		{
			base.HandleExit();

			SetDefualtProjectileSpawnStrategy();
		}

		protected override void Awake()
		{
			base.Awake();

			SetDefualtProjectileSpawnStrategy();
		}

		protected override void Start()
		{
			base.Start();

			AnimationEventHandler.OnAttackAction += HandleAttackAction;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			AnimationEventHandler.OnAttackAction -= HandleAttackAction;
		}

		private void OnDrawGizmos()
		{
			if(data == null || !Application.isPlaying) return;

			foreach(var item in data.GetAllAttackData())
			{
				foreach(var point in item.SpawnInfos)
				{
					var pos = transform.position + (Vector3)point.Offset;

					Gizmos.DrawWireSphere(pos, 0.2f);
					Gizmos.color = Color.red;
					Gizmos.DrawLine(pos, pos + (Vector3)point.Direction.normalized);
					Gizmos.color = Color.white;
				}
			}
		}
	}
}