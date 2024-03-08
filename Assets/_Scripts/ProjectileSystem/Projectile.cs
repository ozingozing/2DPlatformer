using Ozing.ProjectileSystem.DataPackages;
using System;
using System.Collections;
using UnityEngine;

namespace Ozing.ProjectileSystem
{
	public class Projectile : MonoBehaviour
	{
		public event Action OnInit;
		public event Action OnReset;

		public event Action<ProjectileDataPackage> OnReceiveDataPackage;

		public Rigidbody2D RB {  get; private set; }

		public void Init()
		{
			OnInit?.Invoke();
		}

		public void Reset()
		{
			OnReset?.Invoke();
		}

		public void SendDataPackage(ProjectileDataPackage dataPackage)
		{
			OnReceiveDataPackage?.Invoke(dataPackage);
		}

		private void Awake()
		{
			RB = GetComponent<Rigidbody2D>();
		}
	}
}