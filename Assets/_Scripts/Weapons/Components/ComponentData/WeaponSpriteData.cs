using Ozing.Weapons.Components.ComponentData.AttackData;
using System;
using System.Collections;
using UnityEngine;

namespace Ozing.Weapons.Components.ComponentData
{
	public class WeaponSpriteData : ComponentData<AttackSprites>
	{
		protected override void SetComponentDependency()
		{
			ComponentDependency = typeof(WeaponSprite);
		}
	}
}