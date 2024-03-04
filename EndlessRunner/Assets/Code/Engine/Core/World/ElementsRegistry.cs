using System;
using System.Collections.Generic;

namespace Game.Engine
{

	public class ElementsRegistry
	{
		private readonly Dictionary<uint, Element> _elements = new();

		internal void Add(Element value)
		{
			if (!_elements.TryAdd(value.Id, value)) {
				throw new ArgumentException($"Element '{value.Id}' is already in the world.");
			}
		}
		
		internal void Remove(uint id)
		{
			if (!_elements.ContainsKey(id)) {
				throw new NullReferenceException($"Element '{id}' doesn't exist in the world.");
			}

			_elements.Remove(id);
		}

		public Element Find(uint id)
		{
			if (!_elements.ContainsKey(id)) {
				throw new NullReferenceException($"Can't find element with id - '{id}'.");
			}

			return _elements[id];
		}
		
		public bool TryFind(uint id, out Element result)
		{
			if (!_elements.ContainsKey(id)) {
				result = null;
				return false;
			}

			result = _elements[id];
			return true;
		}
	}

}