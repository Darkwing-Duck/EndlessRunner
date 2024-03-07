using Game.Configs;
using Game.Engine;
using Game.Presentation.View;
using UnityEngine;

namespace Game.Presentation
{

	public class HeroPresenter : ElementPresenter<HeroConfig, Hero, HeroView>
	{
		protected override string InitializeViewGroup() => "Hero";
		
		public HeroPresenter(Hero model) : base(model) { }

		/// <summary>
		/// Sets LevelPresenter's view as a parent for hero
		/// </summary>
		protected override Transform InitializeViewParent() => 
			Root.FindSinglePresenter<LevelPresenter>().View.ElementsContainer;

		/// <summary>
		/// Cache config of the hero
		/// </summary>
		protected override HeroConfig InitializeConfig() => 
			Configs.Heroes.Get(Model.ConfigId);

		protected override void OnActivate()
		{
			View.name = $"Hero<{Model.Uid}>";
			
			var speedStat = Model.Stats.Find<GameStat.Speed>();
			View.SetSpeed(speedStat.GetValue());

			View.OnCollideWith += OnCollideWith;
		}

		/// <summary>
		/// Calls when hero collides with any of element.
		/// Pushes HeroCollectItemCommand to engine
		/// </summary>
		private void OnCollideWith(ElementUidRef elementUid)
		{
			EngineInput.Push(new HeroCollectItemCommand(Model.Uid, elementUid.Value));
		}

		protected override void OnDeactivate()
		{
			View.OnCollideWith -= OnCollideWith;
		}
	}

}