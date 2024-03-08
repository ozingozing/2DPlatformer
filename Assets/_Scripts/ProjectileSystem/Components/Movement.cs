using System;
using System.Collections;
using UnityEngine;

namespace Ozing.ProjectileSystem.Components
{
	public class Movement : ProjectileComponent
	{
		[field : SerializeField] public bool ApplyContinuously { get; private set; }
		[field: SerializeField] public float Speed { get; private set; }

		protected override void Init()
		{
			base.Init();

			SetVelocity();
		}

		private void SetVelocity() => rb.velocity = Speed * transform.right;

		protected override void FixedUpdate()
		{
			base.FixedUpdate();

			if (!ApplyContinuously) return;

			SetVelocity();
		}

	}
}