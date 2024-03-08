using System;
using System.Collections.Generic;

namespace Game.Engine
{

	public class ElementsRegistry
	{
		private readonly Dictionary<uint, Element> _elements = new();
		private readonly IElementUidGenerator _uidGenerator;

		public ElementsRegistry(IElementUidGenerator uidGenerator) => _uidGenerator = uidGenerator;

		internal uint GenerateNextUid() => _uidGenerator.Next();

		internal void Add(Element value)
		{
			if (!_elements.TryAdd(value.Uid, value)) {
				throw new ArgumentException($"Element '{value.Uid}' is already in the world.");
			}
		}
		
		internal Element Remove(uint uid)
		{
			if (!_elements.ContainsKey(uid)) {
				throw new NullReferenceException($"Element '{uid}' doesn't exist in the world.");
			}

			var result = _elements[uid];
			_elements.Remove(uid);
			return result;
		}

		public Element Find(uint id)
		{
			if (!_elements.ContainsKey(id)) {
				throw new NullReferenceException($"Can't find element with id - '{id}'.");
			}

			return _elements[id];
		}
		
		public TElement Find<TElement>(uint id) where TElement : Element
		{
			var element = Find(id);

			if (element is not TElement) {
				var type = typeof(TElement);
				throw new NullReferenceException($"Can't find '{type.Name}' with id - '{id}'.");
			}

			return (TElement)_elements[id];
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
		
		public bool TryFind<TElement>(uint id, out TElement result) where TElement : Element
		{
			if (!TryFind(id, out var element)) {
				result = null;
				return false;
			}

			if (element is not TElement) {
				result = null;
				return false;
			}

			result = (TElement)_elements[id];
			return true;
		}

		public void ForEach(Action<Element> callback)
		{
			foreach (var element in _elements.Values) {
				callback?.Invoke(element);
			}
		}
	}

}