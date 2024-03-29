using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozing.CoreSystem
{
	public class CoreComponent : MonoBehaviour, ILogicUpdate
	{
		protected Core core;


		protected virtual void Awake()
		{
			core = transform.parent.GetComponent<Core>();
			//Debug.Log(this.gameObject.transform.parent.name);
			if (core == null)
			{
				Debug.LogError("There is no Core on the parent");
			}
			core.AddComponent(this);
		}

		public virtual void LogicUpdate() { }
	}
}