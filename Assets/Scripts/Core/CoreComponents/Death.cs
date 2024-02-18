using Unity.VisualScripting;
using UnityEngine;

public class Death : CoreComponent
{
    [SerializeField] private GameObject[] deathParicles;

    private ParticleManager ParticleManager => particleManager ? particleManager : core.GetCoreComponent<ParticleManager>(ref particleManager);
    private ParticleManager particleManager;

    private Stats Stats => stats ? stats : core.GetCoreComponent<Stats>(ref stats);
    private Stats stats;

    public void Die()
    {
        foreach(var particle in deathParicles)
        {
            ParticleManager.StartParticles(particle);
        }

        core.transform.parent.gameObject.SetActive(false);
    }

	private void OnEnable()
	{
        Stats.OnHealthZero += Die;
	}

	private void OnDisable()
	{
		Stats.OnHealthZero -= Die;
	}
}
