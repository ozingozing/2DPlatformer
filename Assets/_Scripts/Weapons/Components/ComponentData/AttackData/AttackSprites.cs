using System;
using System.Collections;
using UnityEngine;

namespace Ozing.Weapons.Components.ComponentData.AttackData
{
	[Serializable]
	public class AttackSprites
	{
		[field: SerializeField] public Sprite[] Sprites {  get; private set; }
	}
}