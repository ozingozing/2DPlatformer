using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozing.Weapons.Components.ComponentData.AttackData
{
    [Serializable]
    public class AttackPoiseDamage : AttackData
    {
        [field : SerializeField] public float Amount { get; private set; }
    }
}
