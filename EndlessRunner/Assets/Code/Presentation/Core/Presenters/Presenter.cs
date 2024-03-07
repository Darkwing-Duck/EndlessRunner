using System;
using Game.Configs;
using Game.Engine;
using Game.Presentation.View;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Game.Presentation
{

	public abstract class Presenter
	{
		/// <summary>
		/// Key of the presenter
		/// </summary>
		public readonly object Key;
		
		/// <summary>
		/// Root of the presentation system
		/// </summary>
		public PresentationRoot Root { get; internal set; }
		
		/// <summary>
		/// Provides access to all the game configs
		/// </summary>
		public ConfigsRegistry Configs { get; internal set; }
		
		/// <summary>
		/// Provides interface to push commands to engine
		/// </summary>
		public IEngineInput EngineInput { get; internal set; }
		
		/// <summary>
		/// Provides access to world state (Read Only)
		/// </summary>
		public World World { get; internal set; }

		protected Presenter(object key) => Key = key;

		internal abstract void Build();
		internal abstract void Destruct();
		internal abstract void Configure();
	}
	
	public abstract class Presenter<TView> : Presenter where TView : MonoBehaviour
	{
		/// <summary>
		/// The view is controlling by the presenter
		/// </summary>
		public TView View { get; protected set; }
		
		/// <summary>
		/// Group name of the asset. Used to build asset loading path.
		/// ViewPath: Prefabs/{ViewGroup}/P_{ViewGroup}_{key}.prefab
		/// </summary>
		protected string ViewGroup { get; private set; }
		
		/// <summary>
		/// Key of the asset. Used to build asset loading path.
		/// ViewPath: Prefabs/{ViewGroup}/P_{ViewGroup}_{ViewKey}.prefab
		/// </summary>
		protected string ViewKey { get; private set; }
		
		/// <summary>
		/// Indicates a parent transform for the presenter's view
		/// </summary>
		protected Transform ViewParent { get; private set; }

		public Presenter(object key) : base(key) { }

		protected virtual Transform InitializeViewParent() { return null; }
		protected abstract string InitializeViewKey();
		protected abstract string InitializeViewGroup();

		private TView LoadViewWith(string key)
		{
			var viewKey = $"Prefabs/{ViewGroup}/P_{ViewGroup}_{key}.prefab";
			var prefab = Addressables.LoadAssetAsync<GameObject>(viewKey).WaitForCompletion();
			var instance = Object.Instantiate(prefab, ViewParent);

			if (!instance.TryGetComponent<TView>(out var component)) {
				throw new NullReferenceException($"Can't find component '{typeof(TView).Name}' on asset '{viewKey}'.");
			}
			
			return component;
		}

		internal override void Configure() { }

		/// <summary>
		/// Builds the presenter
		/// </summary>
		internal override void Build()
		{
			ViewParent = InitializeViewParent();
			ViewGroup = InitializeViewGroup();
			ViewKey = InitializeViewKey();
			View = LoadViewWith(ViewKey);

			// cache presenter key on gameObject to simplify searching a presenter by gameObject
			var presenterKey = View.gameObject.AddComponent<PresenterKeyReference>();
			presenterKey.Value = Key;
			
			OnActivate();
		}

		internal override void Destruct()
		{
			OnDeactivate();
			Object.Destroy(View.gameObject);
		}

		/// <summary>
		/// Calls when the presenter is ready and it's view is already loaded.
		/// </summary>
		protected virtual void OnActivate()
		{ }
		
		/// <summary>
		/// Calls before view of the presenter will be destroyed.
		/// </summary>
		protected virtual void OnDeactivate()
		{ }
	}

}