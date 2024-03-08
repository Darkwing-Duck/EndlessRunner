using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Engine
{

	public class Stat
	{
		public readonly float BaseValue;
		public readonly float Min;
		public readonly float Max;

		private List<IStatModifier> _modifiers;

		public Stat(float baseValue, float min = 0f, float max = float.MaxValue)
		{
			BaseValue = baseValue;
			Min = min;
			Max = max;
			_modifiers = new();
		}

		/// <summary>
		/// Adds modifier to the stat.
		/// </summary>
		internal void AddModifier(IStatModifier value) { _modifiers.Add(value); }
		internal void RemoveModifier(IStatModifier value) { _modifiers.Remove(value); }

		public IEnumerable<IStatModifier> FindAllModifiersBy(object group) =>
			_modifiers.FindAll(m => m.Group.Equals(group));

		/// <summary>
		/// Calculates and returns actual stat value taking in account all modifiers 
		/// </summary>
		public float GetValue() =>
			_modifiers
				.Aggregate(BaseValue, (current, modifier) => modifier.Modify(current));
	}

}