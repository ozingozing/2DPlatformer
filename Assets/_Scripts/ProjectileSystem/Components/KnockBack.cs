using Ozing.ProjectileSystem.DataPackages;
using Ozing.Utilities;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Ozing.ProjectileSystem.Components
{
	public class KnockBack : ProjectileComponent
	{
		public UnityEvent OnKnockBack;

		[field: SerializeField] public LayerMask LayerMask { get; private set; }

		private HitBox HitBox => hitBox ? hitBox : projectile.GetProjectileComponent(ref hitBox);
		private HitBox hitBox;

		private int direction;
		private float strength;
		private Vector2 angle;

		private void HandleRaycastHit2D(RaycastHit2D[] hits)
		{
			if (!Active) return;

			direction = (int)Mathf.Sign(transform.right.x);

			foreach (var hit in hits)
			{
				if (!LayerMaskUtilities.IsLayerInColliderGameObject(hit, LayerMask)) continue;

				if(!hit.collider.transform.gameObject.TryGetComponent(out IKnockBackable knockBackable)) continue;

				knockBackable.KnockBack(new Combat.KnockBack.KnockBackData(angle, strength, direction, projectile.gameObject));

				OnKnockBack?.Invoke();

				return;
			}
		}

		protected override void HandleReceiveDataPackage(ProjectileDataPackage dataPackage)
		{
			base.HandleReceiveDataPackage(dataPackage);

			if (dataPackage is not KnockBackDataPackage knockBackDataPackage) return;

			strength = knockBackDataPackage.Strength;
			angle = knockBackDataPackage.Angle;
		}

		protected override void Awake()
		{
			base.Awake();

			HitBox?.OnRaycastHit2D.AddListener(HandleRaycastHit2D);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			HitBox?.OnRaycastHit2D.RemoveListener(HandleRaycastHit2D);
		}
	}
}