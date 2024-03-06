using System;
using Game.Configs;
using Game.Engine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Game.Presentation
{

	public abstract class Presenter
	{
		public readonly object Key;
		public PresentationRoot Root { get; internal set; }
		public ConfigsRegistry Configs { get; internal set; }
		public World World { get; internal set; }

		protected Presenter(object key) => Key = key;

		internal abstract void Build();
		internal abstract void Destruct();
		internal abstract void Configure();
	}
	
	public abstract class Presenter<TView> : Presenter where TView : MonoBehaviour
	{
		public TView View { get; protected set; }
		protected string ViewGroup { get; private set; }
		protected Transform ViewParent { get; private set; }
		protected string ViewKey { get; private set; }

		public Presenter(object key) : base(key) { }

		protected virtual Transform InitializeViewParent() { return null; }
		protected abstract string InitializeViewKey();
		protected abstract string InitializeViewGroup();

		protected virtual TView LoadViewWith(string key)
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

		internal override void Build()
		{
			ViewParent = InitializeViewParent();
			ViewGroup = InitializeViewGroup();
			ViewKey = InitializeViewKey();
			View = LoadViewWith(ViewKey);
			
			OnActivate();
		}

		internal override void Destruct()
		{
			OnDeactivate();
			Object.Destroy(View.gameObject);
		}

		protected virtual void OnActivate()
		{ }
		
		protected virtual void OnDeactivate()
		{ }
	}

}