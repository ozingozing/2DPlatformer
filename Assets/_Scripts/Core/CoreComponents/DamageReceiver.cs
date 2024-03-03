using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozing.CoreSystem
{
	public class DamageReceiver : CoreComponent, IDamageable
	{
		[SerializeField] private GameObject damageParticles;

		private Stats stats;
		private ParticleManager particleManage;
		
		public void Damage(float amount)
		{
			Debug.Log(core.transform.parent.name + " Damaged!");
			stats?.DecreaseHealth(amount);
			particleManage?.StartParticlesWithRandomRotation(damageParticles);
		}

		protected override void Awake()
		{
			base.Awake();

			stats = core.GetCoreComponent<Stats>();
			particleManage = core.GetCoreComponent<ParticleManager>();
		}

	}
}
