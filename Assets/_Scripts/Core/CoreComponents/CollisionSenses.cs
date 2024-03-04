using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Ozing.CoreSystem
{
	public class CollisionSenses : CoreComponent
	{
		protected Movement Movement
		{ get => movement ?? core.GetCoreComponent(ref movement); }
		private Movement movement;



		[SerializeField] private Transform groundCheck;
		[SerializeField] private Transform wallCheck;
		[SerializeField] private Transform ledgeCheckHorizontal;
		[SerializeField] private Transform ledgeCheckVertical;
		[SerializeField] private Transform ceilingCheck;

		[SerializeField] private float groundCheckRadius;
		[SerializeField] private float wallCheckDistance;

		[SerializeField] private LayerMask whatIsGround;

		#region Check Transforms

		public Transform GrounCheck
		{
			get => GenericNotImplementedError<Transform>.TryGet(groundCheck, core.transform.parent.name);
			private set => groundCheck = value;
		}
		public Transform WallCheck
		{
			get => GenericNotImplementedError<Transform>.TryGet(wallCheck, core.transform.parent.name);
			private set => wallCheck = value;
		}
		public Transform LedgeCheckHorizontal
		{
			get => GenericNotImplementedError<Transform>.TryGet(ledgeCheckHorizontal, core.transform.parent.name);
			private set => ledgeCheckHorizontal = value;
		}
		public Transform LedgeCheckVertical
		{
			get => GenericNotImplementedError<Transform>.TryGet(ledgeCheckVertical, core.transform.parent.name);
			private set => ledgeCheckVertical = value;
		}
		public Transform CeilingCheck
		{
			get => GenericNotImplementedError<Transform>.TryGet(ceilingCheck, core.transform.parent.name);
			private set => ceilingCheck = value;
		}

		public float GroundCheckRadius
		{
			get => groundCheckRadius;
			private set => groundCheckRadius = value;
		}
		public LayerMask WhatIsGround
		{
			get => whatIsGround;
			private set => whatIsGround = value;
		}
		public float WallCheckDistance
		{
			get => wallCheckDistance;
			private set => wallCheckDistance = value;
		}

		#endregion

		#region Check Functions

		public bool Ceiling
		{
			get => Physics2D.OverlapCircle(ceilingCheck.position, groundCheckRadius, whatIsGround);
		}
		public bool Ground
		{
			get => Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
		}

		public bool WallFront
		{
			get => Physics2D.Raycast(wallCheck.position, Vector2.right * Movement.FacingDirection,
									wallCheckDistance, whatIsGround);
		}

		public bool LedgeHorizontal
		{
			get => Physics2D.Raycast(ledgeCheckHorizontal.position, Vector2.right * Movement.FacingDirection,
				wallCheckDistance, whatIsGround);
		}
		public bool LedgeVertical
		{
			get => Physics2D.Raycast(ledgeCheckVertical.position, Vector2.down,
				wallCheckDistance, whatIsGround);
		}

		public bool WallBack
		{
			get => Physics2D.Raycast(wallCheck.position, Vector2.right * -Movement.FacingDirection,
									wallCheckDistance, whatIsGround);
		}


		#endregion
	}
}