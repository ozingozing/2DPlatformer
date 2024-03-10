using Ozing.Utilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Ozing.ProjectileSystem.Components
{
	[RequireComponent(typeof(HitBox))]
	public class StickToLayer : ProjectileComponent
	{
		//This is similar to a delegate, but you can see what it is linked to.
		[SerializeField] public UnityEvent setStuck;
		[SerializeField] public UnityEvent setUnstuck;

		[field : SerializeField] public LayerMask LayerMask { get; private set; }
		[field : SerializeField] public string InactiveSortingLayerName { get; private set; }
		[field : SerializeField] public float CheckDistance { get; private set; }

		private bool isStuck;
		private bool subscribedToDisableNotifier;

		private HitBox hitBox;

		private string activeSortingLayerName;

		private SpriteRenderer SR;

		private OnDisableNotifier onDisableNotifier;

		private Transform referenceTransform;
		private Transform _transform;

		private Vector3 offsetPosition;
		private Quaternion offsetRotation;

		private float gravityScale;

		private void HandleRaycastHit2D(RaycastHit2D[] hits)
		{
			Debug.Log("Hit!!!!");
			if (isStuck) return;

			SetStuck();

			var lineHit = Physics2D.Raycast(_transform.position, _transform.right, CheckDistance, LayerMask);

			if(lineHit)
			{
				SetReferenceTransformAndPoint(lineHit.transform, lineHit.point);
				return;
			}

			foreach(var hit in hits)
			{
				if(!LayerMaskUtilities.IsLayerInColliderGameObject(hit, LayerMask)) continue;

				SetReferenceTransformAndPoint(hit.transform, hit.point);
				return;
			}

			SetUnstuck();
		}

		private void SetReferenceTransformAndPoint(Transform newReferenceTransform, Vector2 newPoint)
		{
			if (newReferenceTransform.TryGetComponent(out onDisableNotifier))
			{
				onDisableNotifier.OnDisableEvent += HandleDisableNotifier;
				subscribedToDisableNotifier = true;
			}

			_transform.position = newPoint;

			referenceTransform = newReferenceTransform;
			offsetPosition = _transform.position - referenceTransform.position;
			offsetRotation = Quaternion.Inverse(referenceTransform.rotation) * _transform.rotation;
		}

		private void SetStuck()
		{
			isStuck = true;

			SR.sortingLayerName = InactiveSortingLayerName;
			rb.velocity = Vector3.zero;
			rb.bodyType = RigidbodyType2D.Static;

			setStuck?.Invoke();
		}

		private void SetUnstuck()
		{
			isStuck = false;

			SR.sortingLayerName = activeSortingLayerName;
			rb.bodyType = RigidbodyType2D.Dynamic;
			rb.gravityScale = gravityScale;

			setUnstuck?.Invoke();
		}

		

		private void HandleDisableNotifier()
		{
			SetUnstuck();

			if (!subscribedToDisableNotifier) return;

			onDisableNotifier.OnDisableEvent -= HandleDisableNotifier;
			subscribedToDisableNotifier = false;
		}

		protected override void ResetProjectile()
		{
			base.ResetProjectile();

			SetUnstuck();
		}

		protected override void Awake()
		{
			base.Awake();

			gravityScale = rb.gravityScale;
			_transform = transform;
			SR = GetComponentInChildren<SpriteRenderer>();
			activeSortingLayerName = SR.sortingLayerName;
			hitBox = GetComponent<HitBox>();
			
			hitBox.OnRaycastHit2D.AddListener(HandleRaycastHit2D);
		}

		protected override void Update()
		{
			base.Update();

			if (!isStuck) return;

			if(!referenceTransform)
			{
				SetUnstuck();
				return;
			}

			var referenceRotation = referenceTransform.rotation;
			_transform.position = referenceTransform.position + referenceRotation * offsetPosition;
			_transform.rotation = referenceRotation * offsetRotation;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			hitBox.OnRaycastHit2D.RemoveListener(HandleRaycastHit2D);

			if(subscribedToDisableNotifier)
			{
				onDisableNotifier.OnDisableEvent -= HandleDisableNotifier;
			}
		}
	}
}