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
		private event Action<Collider2D[]> OnDetectedCollider2D;

		private CoreComp<CoreSystem.Movement> movement;

		private Vector2 offset;

		private Collider2D[] detected;

		protected override void Start()
		{
			base.Start();

			movement = new CoreComp<CoreSystem.Movement>(Core);
		}

		private void HandleAttackAction()
		{
			offset.Set(
					transform.position.x + (currentAttackData.HitBox.center.x * movement.Comp.FacingDirection),
					transform.position.y + currentAttackData.HitBox.center.y
				);
			detected = Physics2D.OverlapBoxAll(offset, currentAttackData.HitBox.size, 0f, data.DetectableLayers);

			if (detected.Length == 0) return;

			OnDetectedCollider2D?.Invoke(detected);

			foreach (var item in detected)
			{
				Debug.Log(item.name);
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			eventHandler.OnAttackAction += HandleAttackAction;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			eventHandler.OnAttackAction -= HandleAttackAction;
		}

		private void OnDrawGizmos()
		{
			if(data == null) return;

			foreach (var item in data.AttackData)
			{
				if (!item.Debug) continue;
				Gizmos.DrawWireCube(transform.position +
					new Vector3(item.HitBox.center.x * movement.Comp.FacingDirection, item.HitBox.center.y),
					item.HitBox.size);
			}
		}
	}
}