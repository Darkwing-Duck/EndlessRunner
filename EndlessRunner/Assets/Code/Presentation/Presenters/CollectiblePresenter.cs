using Game.Configs;
using Game.Engine;
using Game.Presentation.View;
using UnityEngine;

namespace Game.Presentation
{

	public class CollectiblePresenter : ElementPresenter<CollectibleConfig, Collectible, CollectibleView>
	{
		protected override string InitializeViewGroup() => "Collectible";
		public CollectiblePresenter(Collectible model) : base(model) { }

		/// <summary>
		/// Sets LevelPresenter's view as a parent for hero
		/// </summary>
		protected override Transform InitializeViewParent() => 
			Root.FindSinglePresenter<LevelPresenter>().View.transform;

		protected override CollectibleConfig InitializeConfig() => 
			Configs.Collectibles.Get(Model.ConfigId);

		protected override void OnActivate()
		{
			View.name = $"Collectible<{Model.Uid}>";
		}
	}

}