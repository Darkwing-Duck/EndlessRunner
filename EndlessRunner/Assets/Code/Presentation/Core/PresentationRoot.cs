using System;
using System.Collections.Generic;
using Game.Common;
using Game.Configs;
using Game.Engine;
using UnityEngine;

namespace Game.Presentation
{
	
	/// <summary>
	/// The root of presentation layer.
	/// It is OListenerProvider, so it can be passed to engine reaction to give
	/// all presenters ability to handle command results of certain type.
	///
	/// Also if presenter realizes interface IUpdatable its method Update will be called. 
	/// </summary>
	public class PresentationRoot : IUpdatable, IListenersProvider
	{
		// reference to configs
		private ConfigsRegistry _configsRegistry;
		
		// reference to engine input
		private IEngineInput _engineInput;
		
		// reference to world state
		private World _world;
		
		// presenters map
		private Dictionary<object, Presenter> _presenters = new();

		public PresentationRoot(World world, ConfigsRegistry configsRegistry, IEngineInput engineInput) {
			_world = world;
			_configsRegistry = configsRegistry;
			_engineInput = engineInput;
		}

		/// <summary>
		/// Updates all presenters that realize IUpdatable interface
		/// </summary>
		public void Update()
		{
			foreach (var presenter in _presenters.Values) {
				if (presenter is IUpdatable castedPresenter) {
					castedPresenter.Update();
				}
			}
		}
		
		/// <summary>
		/// Adds presenter to the display list.
		/// Configures presenter and creates a view for the presenter.
		/// </summary>
		/// <param name="presenter">Presenter to be displayed</param>
		public void Add(Presenter presenter)
		{
			if (_presenters.ContainsKey(presenter.Key)) {
				throw new Exception($"Presenter with the key '{presenter.Key}' is already exist");
			}
			
			_presenters.Add(presenter.Key, presenter);
			
			// fill up dependencies
			presenter.Root = this;
			presenter.Configs = _configsRegistry;
			presenter.World = _world;
			presenter.EngineInput = _engineInput;
			
			// make custom presenter configuration
			presenter.Configure();
			
			// build the presenter and create view
			presenter.Build();
		}
		
		/// <summary>
		/// Removes presenter from display list.
		/// </summary>
		public void Remove(object key)
		{
			if (!_presenters.ContainsKey(key)) {
				throw new Exception($"Presenter '{key}' is not exist");
			}

			var presenter = _presenters[key];
			presenter.Destruct();
			_presenters.Remove(key);
		}
		
		/// <summary>
		/// Searching the first presenter of type T in the display list.
		/// </summary>
		public T FindSinglePresenter<T>() where T : Presenter
		{
			foreach (var presenter in _presenters.Values) {
				if (presenter is T casted) {
					return casted;
				}
			}
			
			return default;
		}

		/// <summary>
		/// Searching the first presenter of type T in the display list.
		/// </summary>
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
		
		/// <summary>
		/// Searching a presenter of type T with specified key in the display list.
		/// </summary>
		public bool TryFindPresenter<T>(object key, out T result) where T : Presenter
		{
			if (!_presenters.ContainsKey(key)) {
				result = default;
				return false;
			}

			result = (T)_presenters[key];
			return true;
		}
		
		/// <summary>
		/// Searching a presenter of type T with specified key in the display list.
		/// </summary>
		public T FindPresenter<T>(object key) where T : Presenter
		{
			if (!_presenters.ContainsKey(key)) {
				return default;
			}
			
			return (T)_presenters[key];
		}

		/// <summary>
		/// Realization of IListenersProvider interface.
		/// Returns all presenters that implements interface IListener of specified command result 'TCmdResult'
		/// </summary>
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