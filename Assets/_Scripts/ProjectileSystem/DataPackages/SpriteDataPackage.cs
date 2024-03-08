using System;
using System.Collections;
using UnityEngine;

namespace Ozing.ProjectileSystem.DataPackages
{
	[Serializable]
	public class SpriteDataPackage : ProjectileDataPackage
	{
		[field : SerializeField] public Sprite Sprite {  get; private set; }
	}
}