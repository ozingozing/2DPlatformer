using Ozing.Weapons.Components.ComponentData;
using Ozing.Weapons.Components.ComponentData.AttackData;
using System;
using System.Collections;
using UnityEngine;

namespace Ozing.Weapons.Components
{
	public class WeaponSprite : WeaponComponent<WeaponSpriteData, AttackSprites>
	{
		private SpriteRenderer baseSpriteRender;
		private SpriteRenderer weaponSpriteRender;


		private int currentWeaponSpriteIndex;


		protected override void HandleEnter()
		{
			base.HandleEnter();
			currentWeaponSpriteIndex = 0;
		}

		private void HandleBaseSpriteChange(SpriteRenderer sr)
		{
			if (!isAttackActive)
			{
				weaponSpriteRender.sprite = null;
				return;
			}
			if(currentWeaponSpriteIndex < currentAttackData.Sprites.Length)
			{
				weaponSpriteRender.sprite = currentAttackData.Sprites[currentWeaponSpriteIndex];

				currentWeaponSpriteIndex++;
			}
			else
			{
				Debug.LogWarning($"{weapon.name} weapon sprites length mismatch");
				return;
			}
		}

		protected override void Start()
		{
			base.Start();

			baseSpriteRender = weapon.BaseGO.GetComponent<SpriteRenderer>();
			weaponSpriteRender = weapon.WeaponSpriteGO.GetComponent<SpriteRenderer>();

			baseSpriteRender.RegisterSpriteChangeCallback(HandleBaseSpriteChange);
		}
		

		protected override void OnDestroy()
		{
			base.OnDestroy();
			baseSpriteRender.UnregisterSpriteChangeCallback(HandleBaseSpriteChange);
		}
	}
}