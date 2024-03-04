using System.Collections.Generic;
using System.Linq;

namespace Game.Engine
{

	public class Stat
	{
		public readonly float BaseValue;

		private List<IStatModifier> _modifiers;

		public Stat(float baseValue)
		{
			BaseValue = baseValue;
			_modifiers = new();
		}

		internal void AddModifier(IStatModifier value)
		{
			_modifiers.Add(value);
		}
		
		internal void RemoveModifier(IStatModifier value)
		{
			_modifiers.Remove(value);
		}

		/// <summary>
		/// Calculates and returns actual stat value taking in account all modifiers 
		/// </summary>
		public float GetValue() => _modifiers
			.Aggregate(BaseValue, (current, modifier) => modifier.Modify(current));
	}

}