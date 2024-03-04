using System.Collections;
using UnityEngine;

namespace Ozing.CoreSystem
{
	public class PoiseDamageReceiver : CoreComponent, IPoiseDamageable
	{
		private Stats Stats => stats ? stats : core.GetCoreComponent(ref stats);
		private Stats stats;

		public void DamagePoise(float amount)
		{
			Stats.Poise.Decrease(amount);
		}

		protected override void Awake()
		{
			base.Awake();
		}

	}
}