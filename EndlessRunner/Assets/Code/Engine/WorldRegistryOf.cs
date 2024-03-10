using System.Collections.Generic;

namespace Game.Engine
{

	/// <summary>
	/// Base class of container for world based registry of entity types.
	/// </summary>
	public class WorldRegistryOf<T> where T : WorldEntity
	{
		private readonly Dictionary<uint, T> _map = new();

		internal void Add(T entity) { _map.Add(entity.Uid, entity); }
		internal void Remove(uint entityUid) { _map.Remove(entityUid); }
		
		public T Get(uint id) => _map[id];
		public bool TryGet(uint id, out T value) => _map.TryGetValue(id, out value);

		public IEnumerable<T> GetAll() { return _map.Values; }

		public int Count => _map.Count;
	}

}