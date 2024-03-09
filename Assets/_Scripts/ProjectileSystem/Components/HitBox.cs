using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Ozing.ProjectileSystem.Components
{
	public class HitBox : ProjectileComponent
	{
		public UnityEvent<RaycastHit2D[]> OnRaycastHit2D;

		[field: SerializeField] public Rect HitBoxRect { get; private set; }
		[field: SerializeField] public LayerMask LayerMask { get; private set; }

		private RaycastHit2D[] hits;
		private float checkDistance;
		private Transform _transform;

		private void CheckHitBox()
		{
			hits = Physics2D.BoxCastAll(
					transform.TransformPoint(HitBoxRect.center),
					HitBoxRect.size, _transform.rotation.eulerAngles.z,
					_transform.right, checkDistance, LayerMask
				);

			if (hits.Length <= 0) return;

			OnRaycastHit2D?.Invoke( hits );
		}

		protected override void Awake()
		{
			base.Awake();

			_transform = transform;
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();

			checkDistance = rb.velocity.magnitude * Time.deltaTime;

			CheckHitBox();
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;

			Matrix4x4 rotationMatrix
				= Matrix4x4.TRS(
					transform.position, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z),
					Vector3.one
				);
			Gizmos.matrix = rotationMatrix;
			Gizmos.DrawWireCube(HitBoxRect.center, HitBoxRect.size);
		}
	}
}