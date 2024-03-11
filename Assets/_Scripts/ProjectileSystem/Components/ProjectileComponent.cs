using Ozing.ProjectileSystem.DataPackages;
using System.Collections;
using UnityEngine;

namespace Ozing.ProjectileSystem.Components
{
	public class ProjectileComponent : MonoBehaviour
	{
		protected Projectile projectile;
		protected Rigidbody2D rb => projectile.RB;
		public bool Active {  get; private set; }

		protected virtual void Init()
		{
			SetActive(true);
		}

		protected virtual void ResetProjectile()
		{
			
		}

		protected virtual void HandleReceiveDataPackage(ProjectileDataPackage dataPackage)
		{

		}

		public virtual void SetActive(bool value) => Active = value;

		public virtual void SetActiveNextFrame(bool value)
		{
			StartCoroutine(SetActiveNextFrameCorutine(value));
		}

		public IEnumerator SetActiveNextFrameCorutine(bool value)
		{
			yield return null;
			SetActive(value);
		}

		protected virtual void Awake()
		{
			projectile = GetComponent<Projectile>();
			if (projectile == null) { Debug.Log("There is no Projectile this GO!!"); }
			projectile.AddProjectileComponent(this);

			projectile.OnInit += Init;
			projectile.OnReset += ResetProjectile;
			projectile.OnReceiveDataPackage += HandleReceiveDataPackage;
		}

		protected virtual void Update()
		{

		}

		protected virtual void FixedUpdate()
		{

		}

		protected virtual void OnDestroy()
		{
			projectile.OnInit -= Init;
			projectile.OnReset -= ResetProjectile;
			projectile.OnReceiveDataPackage -= HandleReceiveDataPackage;
		}
	}
}