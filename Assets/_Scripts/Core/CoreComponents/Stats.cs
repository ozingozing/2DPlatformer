using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozing.CoreSystem.StatsSystem;

namespace Ozing.CoreSystem
{
	public class Stats : CoreComponent
	{
		[field : SerializeField] public Stat Health { get; private set; }
		[field : SerializeField] public Stat Poise { get; private set; }

		[SerializeField] private float poiseRecoverRate;

		protected override void Awake()
		{
			base.Awake();

			Health.Init();
			Poise.Init();
		}

		private void Update()
		{
			if (Poise.CurrentValue.Equals(Poise.MaxValue)) return;

			Poise.Increase(poiseRecoverRate * Time.deltaTime);
		}
	}
}