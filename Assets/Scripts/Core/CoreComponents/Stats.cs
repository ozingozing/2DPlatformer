using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : CoreComponent
{
	public event Action OnHealthZero;

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
			//?는 null Check해주는거
			OnHealthZero?.Invoke();

			Debug.Log("Health is zeor!");
		}
	}

	public void IncreaseHealth(float amount)
	{
		currenHealth = Mathf.Clamp(currenHealth + amount, 0, maxHealth);
	}
}
