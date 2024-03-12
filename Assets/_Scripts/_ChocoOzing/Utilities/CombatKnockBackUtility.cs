using Ozing.Combat.KnockBack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozing.Utilities
{
	public static class CombatKnockBackUtility
	{
		public static bool TryKnockBack(GameObject gameObject, KnockBackData data, out IKnockBackable knockBackable)
		{
			if(gameObject.TryGetComponentInChildren(out knockBackable))
			{
				knockBackable.KnockBack(data);
				return true;
			}
			return false;
		}
		
		public static bool TryKnockBack(Component component, KnockBackData data, out IKnockBackable knockBackable)
		{
			return TryKnockBack(component.gameObject, data, out knockBackable);
		}

		public static bool TryKnockBack(IEnumerable<GameObject> gameObjects, KnockBackData data, out List<IKnockBackable> knockBackables)
		{
			var hasKnockBack = false;
			knockBackables = new List<IKnockBackable>();

            foreach (var item in gameObjects)
            {
                if(TryKnockBack(item, data, out var knockBackable))
				{
					knockBackables.Add(knockBackable);
					hasKnockBack = true;
				}
            }
			return hasKnockBack;
        }

		public static bool TryKnockBack(IEnumerable<Component> components, KnockBackData data, out List<IKnockBackable> knockBackables)
		{
			var hasKnockBack = false;
			knockBackables = new List<IKnockBackable>();

			foreach (var item in components)
			{
				if(TryKnockBack(item, data, out var knockBackable))
				{
					knockBackables.Add(knockBackable);
					return true;
				}
			}
			return hasKnockBack;
		}
	}
}