using Ozing.Weapons.Components.ComponentData.AttackData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozing.Weapons.Components.ComponentData
{
	public class KnockBackData : ComponentData<AttackKnockBack>
	{
		protected override void SetComponentDependency()
		{
			ComponentDependency = typeof(KnockBack);
		}
	}
}
