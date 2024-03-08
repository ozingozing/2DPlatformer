using System;
using System.Collections;
using UnityEngine;

namespace Ozing.ProjectileSystem.DataPackages
{
	[Serializable]
	public class PoiseDamageDataPackage : ProjectileDataPackage
	{
		[field: SerializeField] public float Amount {  get; private set; }
	}
}