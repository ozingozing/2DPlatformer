using System.Collections;
using UnityEngine;

namespace Ozing.Weapons.Components.ComponentData
{
	public class InputHoldData : ComponentData
	{
		protected override void SetComponentDependency()
		{
			ComponentDependency = typeof(InputHold);
		}
	}
}