using Unity.VisualScripting;
using UnityEngine;

namespace Ozing.CoreSystem
{
	public class Death : CoreComponent
	{
		[SerializeField] private GameObject[] deathParicles;

		private ParticleManager ParticleManager => particleManager ? particleManager : core.GetCoreComponent(ref particleManager);
		private ParticleManager particleManager;

		private Stats Stats => stats ? stats : core.GetCoreComponent(ref stats);
		private Stats stats;

		public void Die()
		{
			foreach (var particle in deathParicles)
			{
				ParticleManager.StartParticles(particle);
			}

			core.transform.parent.gameObject.SetActive(false);
		}

		private void OnEnable()
		{
			Stats.Health.OnCurrenValueZero += Die;
		}

		private void OnDisable()
		{
			Stats.Health.OnCurrenValueZero -= Die;
		}
	}
}