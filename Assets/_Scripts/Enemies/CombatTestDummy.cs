using Ozing.Combat.Damage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTestDummy : MonoBehaviour, IDamageable
{
	[SerializeField] private GameObject hitParticles;

    private Animator anim;

	public void Damage(DamageData data)
	{
		Debug.Log(data.Amount + " Damage taken");

		Instantiate(hitParticles, transform.position, Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f)));
		anim.SetTrigger("damage");
		Destroy(gameObject);
	}

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}
}
