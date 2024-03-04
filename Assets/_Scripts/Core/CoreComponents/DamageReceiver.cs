using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozing.CoreSystem
{
	public class DamageReceiver : CoreComponent, IDamageable
	{
		[SerializeField] private GameObject damageParticles;

		private Stats Stats => stats ? stats : core.GetCoreComponent(ref stats);
		private Stats stats;
		private ParticleManager ParticleManager => particleManage ? particleManage : core.GetCoreComponent(ref particleManage);
		private ParticleManager particleManage;
		
		public void Damage(float amount)
		{
			Debug.Log(core.transform.parent.name + " Damaged!");
			Stats.Health.Decrease(amount);
			ParticleManager?.StartParticlesWithRandomRotation(damageParticles);
		}

		protected override void Awake()
		{
			base.Awake();

			//stats = core.GetCoreComponent<Stats>();
			//particleManage = core.GetCoreComponent<ParticleManager>();
		}

	}
}
