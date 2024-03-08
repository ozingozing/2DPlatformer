using Ozing.ObjectPoolSystem;
using Ozing.ProjectileSystem.DataPackages;
using System.Collections;
using UnityEngine;

namespace Ozing.ProjectileSystem
{
	public class ProjectileTest : MonoBehaviour
	{
		public Projectile ProjectilePrefab;

		public DamageDataPackage DamageDataPackage;

		public float ShotCooldown;

		private ObjectPools objectPools = new ObjectPools();

		private float lastFireTime;

		private void Start()
		{
			if (!ProjectilePrefab)
			{
				Debug.LogWarning("No Projectile To Test");
				return;
			}

			FireProjectile();
		}

		private void FireProjectile()
		{
			var projectile = objectPools.GetPool(ProjectilePrefab).GetObject();

			projectile.Reset();

			projectile.transform.position = transform.position;
			projectile.transform.rotation = transform.rotation;

			projectile.SendDataPackage(DamageDataPackage);

			projectile.Init();

			lastFireTime = Time.time;
		}

		private void Update()
		{
			if (Time.time >= lastFireTime + ShotCooldown)
			{
				FireProjectile();
			}
		}

		[ContextMenu("Destroy Pools")]
		private void DestroyPool()
		{
			lastFireTime = Mathf.Infinity;
			objectPools.Release();
		}
	}
}