using System.Collections.Generic;

namespace Ozing.ModifireSystem
{
	public class Modifiers<TModifierType, TValueType> where TModifierType : Modifier<TValueType>
	{
		private readonly LinkedList<TModifierType> modifierList = new LinkedList<TModifierType>();

		public TValueType ApplyAllModifiers(TValueType initialValue)
		{
			var modifiedValue = initialValue;

			foreach (var modifier in modifierList)
			{
				modifiedValue = modifier.ModifyValue(modifiedValue);
			}

			return modifiedValue;
		}

		public void AddModifier(TModifierType modifier) => modifierList.AddLast(modifier);

		public void RemoveModifier(TModifierType modifier) => modifierList.Remove(modifier);
	}
}