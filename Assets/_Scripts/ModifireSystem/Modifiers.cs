using System.Collections.Generic;

namespace Ozing.ModifireSystem
{
	public class Modifiers<TValueType>
	{
		private readonly List<Modifier<TValueType>> modifierList = new List<Modifier<TValueType>>();
		//private readonly List<TModifierTypeClass> modifierList = new List<TModifierTypeClass>();

		public TValueType ApplyAllModifiers(TValueType initialValue)
		{
			var modifiedValue = initialValue;

			foreach (var modifier in modifierList)
			{
				modifiedValue = modifier.ModifyValue(modifiedValue);
			}

			return modifiedValue;
		}

		public void AddModifier(Modifier<TValueType> modifier) => modifierList.Add(modifier);

		public void RemoveModifier(Modifier<TValueType> modifier) => modifierList.Remove(modifier);
	}
}