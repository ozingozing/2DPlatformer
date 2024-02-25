using Ozing.Weapons.Components.ComponentData.AttackData;
using System;
using System.Collections;
using UnityEngine;

namespace Ozing.Weapons.Components.ComponentData
{
	public class ActionHitBoxData : ComponentData<AttackActionHitBox>
	{
		[field : SerializeField] public LayerMask DetectableLayers {  get; private set; }
		
	}
}