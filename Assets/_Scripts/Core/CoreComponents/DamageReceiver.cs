using Ozing.Combat.Damage;
using Ozing.ModifireSystem;
using Ozing.Weapons.Components.Modifiers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozing.CoreSystem
{
	public class DamageReceiver : CoreComponent, IDamageable
	{
		[SerializeField] private GameObject damageParticles;

		public Modifiers<Modifier<DamageData>, DamageData> Modifiers { get; } = new();

		private Stats Stats => stats ? stats : core.GetCoreComponent(ref stats);
		private Stats stats;
		private ParticleManager ParticleManager => particleManage ? particleManage : core.GetCoreComponent(ref particleManage);
		private ParticleManager particleManage;


		public void Damage(DamageData data)
		{
			Debug.Log($"Damage Amount Before Modifiers : {data.Amount}");

			data = Modifiers.ApplyAllModifiers(data);

			print($"Damage Amoun Afer Modifiers : {data.Amount}");

			if (data.Amount <= 0f) return;

			Stats.Health.Decrease(data.Amount);
			ParticleManager.StartParticlesWithRandomRotation(damageParticles);
		}

		protected override void Awake()
		{
			base.Awake();

			//stats = core.GetCoreComponent<Stats>();
			//particleManage = core.GetCoreComponent<ParticleManager>();
		}

	}
}
