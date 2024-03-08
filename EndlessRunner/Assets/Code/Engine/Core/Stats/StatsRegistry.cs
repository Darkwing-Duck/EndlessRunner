using System;
using System.Collections.Generic;

namespace Game.Engine
{

	public class StatsRegistry
	{
		private readonly Dictionary<Type, Stat> _stats = new();

		public Stat Register<T>(float withValue, float min = 0f, float max = float.MaxValue) where T : Stat
		{
			var type = typeof(T);

			if (_stats.ContainsKey(type)) {
				throw new Exception($"Stat '{type.Name}' is already registered.");
			}

			var instance = (T)Activator.CreateInstance(type, withValue);
			_stats.Add(type, instance);

			return instance;
		}

		public Stat Find<T>() where T : Stat
		{
			var type = typeof(T);
			
			if (!_stats.ContainsKey(type)) {
				throw new Exception($"Stat '{type.Name}' was not registered.");
			}

			return _stats[type];
		}
		
		public bool TryFind<T>(out Stat result) where T : Stat
		{
			var type = typeof(T);
			
			if (!_stats.ContainsKey(type)) {
				result = null;
				return false;
			}

			result = _stats[type];
			return true;
		}
	}

}