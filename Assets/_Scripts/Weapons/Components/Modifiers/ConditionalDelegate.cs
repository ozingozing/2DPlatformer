using Ozing.Weapons.Components.ComponentData.AttackData;
using System.Collections;
using UnityEngine;

namespace Ozing.Weapons.Components.Modifiers
{
	public delegate bool ConditionalDelegate(Transform source, out DirectionalInformation directionalInformation);
}