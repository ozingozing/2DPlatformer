using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozing.CoreSystem
{
	public class DamageReceiver : CoreComponent, IDamageable
	{
		[SerializeField] private GameObject damageParticles;

		private CoreComp<Stats> stats;
		private CoreComp<ParticleManager> particleManage;
		
		public void Damage(float amount)
		{
			Debug.Log(core.transform.parent.name + " Damaged!");
			stats.Comp?.DecreaseHealth(amount);
			particleManage.Comp?.StartParticlesWithRandomRotation(damageParticles);
		}

		protected override void Awake()
		{
			base.Awake();

			stats = new CoreComp<Stats>(core);
			particleManage = new CoreComp<ParticleManager>(core);
		}

	}
}
