using Game.Configs;
using Game.Engine;
using Game.Presentation.View;
using UnityEngine;

namespace Game.Presentation
{

	/// <summary>
	/// Presenter that displays all collectible elements
	/// </summary>
	public class CollectiblePresenter : ElementPresenter<CollectibleConfig, Collectible, CollectibleView>
	{
		protected override string InitializeViewGroup() => "Collectible";
		public CollectiblePresenter(Collectible model) : base(model) { }

		/// <summary>
		/// Sets LevelPresenter's view as a parent for hero
		/// </summary>
		protected override Transform InitializeViewParent() => 
			Root.FindSinglePresenter<LevelPresenter>().View.ElementsContainer;

		/// <summary>
		/// Initialize collectible config
		/// </summary>
		protected override CollectibleConfig InitializeConfig() => 
			Configs.Collectibles.Get(Model.ConfigId);

		protected override void OnActivate()
		{
			// set the name of gameObject
			View.name = $"Collectible<{Model.Uid}>";
		}
	}

}