using System;
using System.Collections.Generic;
using Game.Common;
using Game.Configs;
using Game.Engine;
using UnityEngine;

namespace Game.Presentation
{
	
	public class PresentationRoot : IUpdatable, IListenersProvider
	{
		private ConfigsRegistry _configsRegistry;
		private World _world;
		private Dictionary<object, Presenter> _presenters = new();
		private Dictionary<MonoBehaviour, Presenter> _presentersByViewMap = new();

		public PresentationRoot(World world, ConfigsRegistry configsRegistry) {
			_world = world;
			_configsRegistry = configsRegistry;
		}

		public void Update()
		{
			foreach (var presenter in _presenters.Values) {
				if (presenter is IUpdatable castedPresenter) {
					castedPresenter.Update();
				}
			}
		}
		
		public void Add(Presenter presenter)
		{
			if (_presenters.ContainsKey(presenter.Key)) {
				throw new Exception($"Presenter with the key '{presenter.Key}' is already exist");
			}
			
			_presenters.Add(presenter.Key, presenter);
			
			presenter.Root = this;
			presenter.Configs = _configsRegistry;
			presenter.World = _world;
			
			presenter.Configure();
			presenter.Build();
		}
		
		public void Remove(object key)
		{
			if (!_presenters.ContainsKey(key)) {
				throw new Exception($"Presenter '{key}' is not exist");
			}

			var presenter = _presenters[key];
			presenter.Destruct();
			_presenters.Remove(key);
		}
		
		public T FindSinglePresenter<T>() where T : Presenter
		{
			foreach (var presenter in _presenters.Values) {
				if (presenter is T casted) {
					return casted;
				}
			}
			
			return default;
		}

		public bool TryFindSinglePresenter<T>(out T result) where T : Presenter
		{
			foreach (var presenter in _presenters.Values) {
				if (presenter is T casted) {
					result = casted;
					return true;
				}
			}

			result = default;
			return false;
		}
		
		public bool TryFindPresenter<T>(object key, out T result) where T : Presenter
		{
			if (!_presenters.ContainsKey(key)) {
				result = default;
				return false;
			}

			result = (T)_presenters[key];
			return true;
		}
		
		public T FindPresenter<T>(object key) where T : Presenter
		{
			if (!_presenters.ContainsKey(key)) {
				return default;
			}
			
			return (T)_presenters[key];
		}

		public IListener<TCmdResult>[] FindAllFor<TCmdResult>() where TCmdResult : CmdResult
		{
			var result = new List<IListener<TCmdResult>>();
			
			foreach (var presenter in _presenters.Values) {
				if (presenter is IListener<TCmdResult> listener) {
					result.Add(listener);
				}
			}

			return result.ToArray();
		}
	}

}