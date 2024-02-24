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

		protected override void Awake()
		{
			base.Awake();

			baseSpriteRender = transform.Find("Base").GetComponent<SpriteRenderer>();
			weaponSpriteRender = transform.Find("WeaponSprite").GetComponent<SpriteRenderer>();


			// TODO: Fix this when we create weapon data
			//baseSpriteRender = weapon.BaseGO.GetComponent<SpriteRenderer>();
			//weaponSpriteRender = weapon.WeaponSpriteGO.GetComponent<SpriteRenderer>();
		}

		//유니티에도 이렇게 event함수를 사용하도록 권장하는 걸 보니깐 사용할 때 연결하고 사용이 끝나면 끊어버리네
		protected override void OnEnable()
		{
			base.OnEnable();
			baseSpriteRender.RegisterSpriteChangeCallback(HandleBaseSpriteChange);
			weapon.OnEnter += HandleEnter;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			baseSpriteRender.UnregisterSpriteChangeCallback(HandleBaseSpriteChange);
			weapon.OnEnter -= HandleEnter;
		}
	}
}