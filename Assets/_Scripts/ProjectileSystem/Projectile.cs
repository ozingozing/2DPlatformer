using Ozing.ProjectileSystem.Components;
using Ozing.ProjectileSystem.DataPackages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ozing.ProjectileSystem
{
	public class Projectile : MonoBehaviour
	{
		private readonly List<ProjectileComponent> ProjectileComponents = new List<ProjectileComponent>();

		public event Action OnInit;
		public event Action OnReset;

		public event Action<ProjectileDataPackage> OnReceiveDataPackage;

		public Rigidbody2D RB {  get; private set; }

		public void AddProjectileComponent(ProjectileComponent projectileComponent)
		{
			if(!ProjectileComponents.Contains(projectileComponent)) ProjectileComponents.Add(projectileComponent);
		}

		public T GetProjectileComponent<T>() where T : ProjectileComponent
		{
			var comp = ProjectileComponents.OfType<T>().FirstOrDefault();
			
			if(comp) return comp;

			comp = GetComponent<T>();

			if (comp) return comp;

			Debug.Log($"{typeof(T)} not found on {transform.parent.name}");
			return null;
		}

		public T GetProjectileComponent<T>(ref T value) where T : ProjectileComponent
		{
			value = GetProjectileComponent<T>();
			return value;
		}


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