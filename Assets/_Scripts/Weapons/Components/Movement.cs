using Ozing.Weapons.Components.ComponentData;
using Ozing.Weapons.Components.ComponentData.AttackData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozing.Weapons.Components
{
    public class Movement : WeaponComponent<MovementData, AttackMovement>
    {
		private CoreSystem.Movement coreMovement;
		private CoreSystem.Movement CoreMovement => coreMovement ? coreMovement : Core.GetCoreComponent(ref coreMovement);

		//private MovementData data;

		private void HandleStartMovement()
        {
			//var currentAttackData = data.AttackData[weapon.CurrentAttackCounter];
			//var currentAttackData = data.AttackData[weapon.CurrentAttackCounter];
			CoreMovement.SetVelocity(currentAttackData.Velocity, currentAttackData.Directon, CoreMovement.FacingDirection);
        }

        private void HandleStopMovement()
        {
			CoreMovement.SetVelocityZero();
        }


		protected override void OnEnable()
		{
			base.OnEnable();

            EventHandler.OnStartMovement += HandleStartMovement;
            EventHandler.OnStopMovement += HandleStopMovement;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			EventHandler.OnStartMovement -= HandleStartMovement;
			EventHandler.OnStopMovement -= HandleStopMovement;
		}
	}
}
