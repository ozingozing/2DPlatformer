using System;
using Ozing.CoreSystem;
using Ozing.Weapons.Components.ComponentData;
using Ozing.Weapons.Components.ComponentData.AttackData;
using System.Collections;
using UnityEngine;

namespace Ozing.Weapons.Components
{
	public class ActionHitBox : WeaponComponent<ActionHitBoxData, AttackActionHitBox>
	{
		public event Action<Collider2D[]> OnDetectedCollider2D;

		private CoreSystem.Movement movement;
		private CoreSystem.Movement Movement => movement ? movement : Core.GetCoreComponent(ref movement);

		private Vector2 offset;

		private Collider2D[] detected;

		protected override void Start()
		{
			base.Start();

			eventHandler.OnAttackAction += HandleAttackAction;
		}

		private void HandleAttackAction()
		{
			offset.Set(
					transform.position.x + (currentAttackData.HitBox.center.x * Movement.FacingDirection),
					transform.position.y + currentAttackData.HitBox.center.y
				);
			detected = Physics2D.OverlapBoxAll(offset, currentAttackData.HitBox.size, 0f, data.DetectableLayers);

			if (detected.Length == 0) return;

			OnDetectedCollider2D?.Invoke(detected);
		}


		protected override void OnDestroy()
		{
			base.OnDestroy();
			eventHandler.OnAttackAction -= HandleAttackAction;
		}

		private void OnDrawGizmos()
		{
			if(data == null) return;

			foreach (var item in data.AttackData)
			{
				if (!item.Debug) continue;
				Gizmos.DrawWireCube(transform.position +
					new Vector3(item.HitBox.center.x * Movement.FacingDirection, item.HitBox.center.y),
					item.HitBox.size);
			}
		}
	}
}