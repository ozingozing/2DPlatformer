using Ozing.Combat.Damage;
using Ozing.ProjectileSystem.DataPackages;
using Ozing.Utilities;
using Ozing.Weapons.Components.ComponentData;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Ozing.ProjectileSystem.Components
{
	public class Damage : ProjectileComponent
	{
		public UnityEvent<IDamageable> OnDamage;
		public UnityEvent<RaycastHit2D> OnRaycastHit;

		[field : SerializeField] public LayerMask LayerMask { get; private set; }
		[field : SerializeField] public bool SetInacativeAfterDamage { get; private set;}
		[field : SerializeField] public float Cooldown { get; private set; }

		protected HitBox HitBox => hitBox ? hitBox : projectile.GetProjectileComponent(ref hitBox);
		private HitBox hitBox;

		private float amount;
		private float lastDamageTime;



		protected override void Init()
		{
			base.Init();

			lastDamageTime = Mathf.NegativeInfinity;
		}


		private void HandleRaycastHit2D(RaycastHit2D[] hits)
		{
			if (!Active) return;

			if(Time.time < lastDamageTime + Cooldown) return;

			foreach (var hit in hits)
			{
				if(!LayerMaskUtilities.IsLayerInColliderGameObject(hit, LayerMask)) continue;

				if(!hit.collider.transform.gameObject.TryGetComponent(out IDamageable damageable)) continue;

				damageable.Damage(new DamageData(amount, projectile.gameObject));

				OnDamage?.Invoke(damageable);
				OnRaycastHit?.Invoke(hit);

				lastDamageTime = Time.time;

				if (SetInacativeAfterDamage) SetActive(false);

				return;
			}
		}

		protected override void HandleReceiveDataPackage(ProjectileDataPackage dataPackage)
		{
			base.HandleReceiveDataPackage(dataPackage);

			if (dataPackage is not DamageDataPackage package) return;

			amount = package.Amount;
		}

		protected override void Awake()
		{
			base.Awake();

			HitBox.OnRaycastHit2D.AddListener(HandleRaycastHit2D);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			HitBox.OnRaycastHit2D.RemoveListener(HandleRaycastHit2D);
		}

	}
}