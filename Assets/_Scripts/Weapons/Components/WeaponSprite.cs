using Ozing.Weapons.Components.ComponentData;
using Ozing.Weapons.Components.ComponentData.AttackData;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Ozing.Weapons.Components
{
	public class WeaponSprite : WeaponComponent<WeaponSpriteData, AttackSprites>
	{
		private SpriteRenderer baseSpriteRender;
		private SpriteRenderer weaponSpriteRender;


		private int currentWeaponSpriteIndex;

		private Sprite[] currentPhaseSprites;

		protected override void HandleEnter()
		{
			base.HandleEnter();
			currentWeaponSpriteIndex = 0;
		}

		private void HandleEnterAttackPhase(AttackPhases phase)
		{
			currentWeaponSpriteIndex = 0;
			currentPhaseSprites = currentAttackData.PhaseSprites.FirstOrDefault(data => data.Phase == phase).Sprites;
		}

		private void HandleBaseSpriteChange(SpriteRenderer sr)
		{
			if (!isAttackActive)
			{
				weaponSpriteRender.sprite = null;
				return;
			}
			if(currentWeaponSpriteIndex >= currentPhaseSprites.Length)
			{
				Debug.LogWarning($"{weapon.name} weapon sprites length mismatch");
				return;
			}
			weaponSpriteRender.sprite = currentPhaseSprites[currentWeaponSpriteIndex];

			currentWeaponSpriteIndex++;
		}

		protected override void Start()
		{
			base.Start();

			baseSpriteRender = weapon.BaseGO.GetComponent<SpriteRenderer>();
			weaponSpriteRender = weapon.WeaponSpriteGO.GetComponent<SpriteRenderer>();

			baseSpriteRender.RegisterSpriteChangeCallback(HandleBaseSpriteChange);
			eventHandler.OnEnterAttackPhase += HandleEnterAttackPhase;
		}
		

		protected override void OnDestroy()
		{
			base.OnDestroy();
			baseSpriteRender.UnregisterSpriteChangeCallback(HandleBaseSpriteChange);
			eventHandler.OnEnterAttackPhase -= HandleEnterAttackPhase;
		}
	}
}