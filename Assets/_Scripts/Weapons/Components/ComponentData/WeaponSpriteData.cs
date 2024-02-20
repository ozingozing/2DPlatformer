using Ozing.Weapons.Components.ComponentData.AttackData;
using System.Collections;
using UnityEngine;

namespace Ozing.Weapons.Components.ComponentData
{
	public class WeaponSpriteData : ComponentData
	{
		[field : SerializeField] public AttackSprites[] AttackData { get; private set; }
	}
}