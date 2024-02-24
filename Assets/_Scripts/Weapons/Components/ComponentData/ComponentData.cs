using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozing.Weapons.Components.ComponentData
{
    [Serializable]
    public class ComponentData
    {

	}
    [Serializable]
    public class ComponentData<T> : ComponentData where T : AttackData.AttackData
    {
        [field : SerializeField] public T[] AttackData {  get; private set; }
    }
}
