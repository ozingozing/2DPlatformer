using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozing.Weapons.Components.ComponentData.AttackData
{
    [Serializable]
    public class AttackMovement
    {
        [field : SerializeField]public Vector2 Directon { get; private set; }
		[field: SerializeField] public float Velocity { get; private set; }
    }
}
