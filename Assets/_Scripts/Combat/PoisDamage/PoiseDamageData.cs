using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozing.Combat.PoiseDamage
{
    public class PoiseDamageData
    {
        public float Amount { get; private set; }
        public GameObject Source { get; private set; }

        public PoiseDamageData(float amount, GameObject source)
        {
            this.Amount = amount;
            this.Source = source;
        }

        public void SetAmount(float amount) => this.Amount = amount;
    }
}
