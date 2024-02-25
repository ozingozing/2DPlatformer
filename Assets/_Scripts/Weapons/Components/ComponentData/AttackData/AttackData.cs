using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozing.Weapons.Components.ComponentData.AttackData
{
	public class AttackData
    {
		[SerializeField, HideInInspector] private string name;

		public void SetAttackName(int i) => name = $"Attack {i}";
	}
}
