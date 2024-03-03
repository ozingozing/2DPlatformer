using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozing.Weapons.Components.ComponentData.AttackData
{
    [Serializable]
    public class AttackKnockBack : AttackData
    {
        [field : SerializeField] public Vector2 Angle { get; private set; }
        [field : SerializeField] public float Strength {  get; private set; }
    }
}
