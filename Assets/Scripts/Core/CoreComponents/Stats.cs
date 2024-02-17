using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : CoreComponent
{
    [SerializeField] private float maxHealth;
    private float currenHealth;

	protected override void Awake()
	{
		base.Awake();

		currenHealth = maxHealth;
	}

	public void DecreaseHealth(float amount)
	{
		currenHealth -= amount;

		if(currenHealth <= 0)
		{
			currenHealth = 0;
			Debug.Log("Health is zeor!");
		}
	}

	public void IncreaseHealth(float amount)
	{
		currenHealth = Mathf.Clamp(currenHealth + amount, 0, maxHealth);
	}
}
