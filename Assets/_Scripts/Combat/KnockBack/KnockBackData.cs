using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozing.Combat.KnockBack
{
    public class KnockBackData
    {
		public Vector2 Angle;
		public float Strength;
		public int Direction;

		public GameObject Source { get; private set; }

		public KnockBackData(Vector2 angle, float strength, int direction, GameObject source)
		{
			this.Angle = angle;
			this.Strength = strength;
			this.Direction = direction;
			this.Source = source;
		}
	}
}
