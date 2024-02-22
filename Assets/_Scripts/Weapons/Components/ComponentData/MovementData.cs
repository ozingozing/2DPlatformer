using Ozing.Weapons.Components.ComponentData.AttackData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozing.Weapons.Components.ComponentData
{
    public class MovementData : ComponentData
    {
        [field : SerializeField] public AttackMovement[] AttackData {  get; private set; }
    }
}
