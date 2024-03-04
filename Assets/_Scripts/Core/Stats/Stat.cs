using System;
using System.Collections;
using UnityEngine;

namespace Ozing.CoreSystem.StatsSystem
{
	[Serializable]
	public class Stat
	{
		public event Action OnCurrenValueZero;

		[field : SerializeField] public float MaxValue { get; private set; }

		public float CurrentValue
		{
			get => currentValue;
			private set
			{
				currentValue = Mathf.Clamp(value, 0f, MaxValue);

				if(currentValue <= 0f)
				{
					OnCurrenValueZero?.Invoke();
				}
			}
		}
		private float currentValue;

		public void Init() => CurrentValue = MaxValue;
		public void Increase(float amount) => CurrentValue += amount;
		public void Decrease(float amount) => CurrentValue -= amount;
	}
}